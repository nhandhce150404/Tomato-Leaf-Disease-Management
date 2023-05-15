using System;
using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;

        public UserController(IUserRepository userRepository, IMapper mapper, AppDbContext context,
            IHostingEnvironment environment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(_mapper.Map<List<Account>>(users));
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPut("ManageRole")]
        public async Task<IActionResult> SetRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == int.Parse(roleDto.AccountId));
            if (account == null) return NotFound($"Account {roleDto.AccountId} cannot be found");

            account.Role = roleDto.Role;
            await _context.SaveChangesAsync();

            return Ok(account);
        }


        [HttpPut("ManageBlocked")]
        public async Task<IActionResult> SetBlocked(BlockedDto blockedDto)

        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == int.Parse(blockedDto.AccountId));
            if (account == null) return NotFound($"Account {blockedDto.AccountId} cannot be found");


            account.IsBlocked = blockedDto.IsBlocked;

            await _context.SaveChangesAsync();


            return Ok(account);
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto user)

        {
            if (user == null)
                return BadRequest(ModelState);

            var userToCreate = _userRepository
                .GetUsers()
                .FirstOrDefault(c => c.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper());

            if (userToCreate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<UserDto>(user);

            if (_userRepository.CreateUser(userMap))
            {
                return Ok("Create User Successfully");
            }

            ModelState.AddModelError("", "Something was wrong while saving");
            return StatusCode(500, ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] Account updatedUser)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return NotFound(ModelState);

            if (user.AccountId != updatedUser.AccountId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UpdateUser(updatedUser, id))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("changeAvatar/{accountId}")]
        public async Task<IActionResult> ChangeAvatar(int accountId, [FromForm] IFormFile postedFile)
        {
            // var postedFile = Request.Form.Files.FirstOrDefault();
            if (postedFile == null) return BadRequest();

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null) return NotFound();

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = Path.GetFileName(postedFile.FileName);
            var uniqueFilePath = Path.Combine(path, fileName);
            var uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            // Check if file name exist, use Windows style rename
            if (System.IO.File.Exists(uniqueFilePath))
            {
                var count = 1;

                var extension = Path.GetExtension(uniqueFilePath);
                var newFullPath = uniqueFilePath;

                while (System.IO.File.Exists(Path.Combine(path, newFullPath)))
                {
                    var tempFileName = $"{uniqueFileName} ({count++})";
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                uniqueFilePath = newFullPath;
                uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            }

            await using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);
            stream.Close();

            // Save image location to database
            _context.Images.Add(new Image { AltText = uniqueFileName, Url = uniqueFilePath });

            // Update account avatar url
            account.AvatarUrl = $@"Uploads/{uniqueFileName}{ext}";

            await _context.SaveChangesAsync();
            return Ok(account.AvatarUrl);
        }

        [HttpDelete("{email}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(id))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpGet("Search")]
        public ActionResult<List<Account>> Search(string search)
        {
            var list = _userRepository.Search(search);

            return Ok(list);
        }
    }
}