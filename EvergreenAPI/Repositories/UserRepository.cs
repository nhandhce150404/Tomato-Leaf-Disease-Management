using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EvergreenAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;


        public UserRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Accounts.SingleOrDefault(u => u.AccountId == id);
            if (user == null)
            {
                return false;
            }

            user.IsBlocked = false;
            _context.Remove(user);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Something is wrong when trying to remove user!");
            }

            return true;
        }

        public Account GetUser(int id)
        {
            var user = _context.Accounts.FirstOrDefault(u => u.AccountId == id);
            if (user == null) return null;

            return user;
        }

        public ICollection<Account> GetUsers()
        {
            var users = _context.Accounts.ToList();
            return users;
        }

        public bool CreateUser(UserDto user)
        {
            if (_context.Accounts.Any(u => u.Email == user.Email))
            {
                return false;
            }

            var password = user.Password;
            var confirmPassword = user.ConfirmPassword;
            if (password != confirmPassword)
            {
                return false;
            }

            if (password.Length < 6)
            {
                return false;
            }

            CreatePasswordHash(user.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var member = new Account
            {
                Username = user.Username,
                FullName = user.FullName,
                Password = password,
                ConfirmPassword = user.ConfirmPassword,
                Email = user.Email,
                Professions = user.Professions,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = user.Role != string.Empty ? user.Role : "User",
                Token = GenerateToken(user.Email, user.Role),
                VerifiedAt = DateTime.Now,
                Chat = "AI: Hello, how can I help you today?",
            };

            _context.Accounts.Add(member);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateUser(Account userDto, int id)
        {
            var user = _context.Accounts.SingleOrDefault(f => f.AccountId == id);
            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(userDto.Username))
                user.Username = userDto.Username;

            if (!string.IsNullOrEmpty(userDto.Bio))
                user.Bio = userDto.Bio;
            if (!string.IsNullOrEmpty(userDto.Role))
                user.Role = userDto.Role;
            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                user.PhoneNumber = userDto.PhoneNumber;
            user.Professions = userDto.Professions;
            user.FullName = userDto.FullName;
            _context.Update(user);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private string GenerateToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.Now;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),

                Expires = now.AddDays(Convert.ToInt32(1)),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }


        public List<Account> Search(string search)
        {
            var d = new List<Account>();
            try
            {
                d = _context.Accounts.Where(d =>
                    d.Username.Contains(search.ToLower()) || d.Email.Contains(search.ToLower()) ||
                    d.FullName.ToLower().Contains(search.ToLower())).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return d;
        }
    }
}