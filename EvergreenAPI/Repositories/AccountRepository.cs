using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EvergreenAPI.Services.EmailService.EmailService;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AccountRepository(IConfiguration config, AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        public Account Login(LoginDto account)
        {
            var user = _context.Accounts.FirstOrDefault(x =>
                x.Password == account.Password && x.Email == account.Email && !x.IsBlocked);

            if (user?.VerifiedAt == null)
            {
                return null;
            }

            var validUser = _context.Accounts
                .FirstOrDefault(x => x.Password == account.Password && x.Email == account.Email);

            if (validUser != null)
            {
                validUser.Token = GenerateToken(validUser.Email, validUser.Role);
                _context.Accounts.Update(validUser);
                _context.SaveChanges();
                return validUser;
            }

            return null;
        }


        public async Task<bool> Register(Account accountDto)
        {
            var userRole = "User";
            var found = _context.Accounts.Any(a => a.Email == accountDto.Email);
            if (found)
            {
                return false;
            }

            var password = accountDto.Password;
            var confirmPassword = accountDto.ConfirmPassword;
            if (password != confirmPassword)
            {
                return false;
            }

            if (password.Length < 7)
            {
                return false;
            }


            CreatePasswordHash(accountDto.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);


            var account = new Account()
            {
                Username = accountDto.Username,
                Email = accountDto.Email,
                Password = accountDto.Password,
                ConfirmPassword = accountDto.ConfirmPassword,
                FullName = accountDto.FullName,
                PhoneNumber = accountDto.PhoneNumber,
                Professions = accountDto.Professions,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User",
                Token = GenerateToken(accountDto.Email, userRole),
                Chat = "AI: Hello, how can I help you today?",
            };


            #region Add Email Template

            try
            {
                #region Send Verification Mail To User

                try
                {
                    var mailContent1 = new MailContent
                    {
                        To = account.Email, //temp email
                        Subject = "Welcome To Evergreen!",
                        Body = account.Token
                    };
                    await _emailService.SendMail(mailContent1);
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }

                #endregion
            }
            catch (Exception)
            {
                return false;
            }

            #endregion

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Account> Verify(string token)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Token == token);
            if (user == null)
            {
                return null;
            }

            if (user.VerifiedAt != null)
            {
                return null;
            }

            user.VerifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<Account> ForgotPassword(string email)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }

            user.PasswordResetToken = GenerateToken(email, "User");
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            #region Add Email Template

            try
            {
                #region Send Verification Mail To User

                try
                {
                    var mailContent1 = new MailContent
                    {
                        To = user.Email, //temp email
                        Subject = "Reset Password!",
                        Body = user.PasswordResetToken
                    };
                    await _emailService.SendMail(mailContent1);
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }

                #endregion
            }
            catch (Exception)
            {
                return null;
            }

            #endregion

            return user;
        }


        public async Task<bool> ResetPassword(ResetPasswordDto request)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.PasswordResetToken == request.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }

            var password = request.Password;
            var confirmPassword = request.ConfirmPassword;
            if (password != confirmPassword)
            {
                return false;
            }

            if (password.Length < 7)
            {
                return false;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            user.Password = password;
            await _context.SaveChangesAsync();
            return true;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac
                .ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
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

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}