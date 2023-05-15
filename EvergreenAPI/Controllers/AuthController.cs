using EvergreenAPI.DTO;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EvergreenAPI.Models;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AuthController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto account)
        {
            var acc = _accountRepository.Login(account);

            if (acc != null)
            {
                return new JsonResult(acc);
            }
            else
            {
                return BadRequest();
            }
        }



        [Route("register")]
        [HttpPost]
        public async Task <IActionResult> Register([FromBody] Account account)
        {
            if (account == null) return BadRequest();

            if (await _accountRepository.Register(account)) return Ok("Register Successfully.");
            else return BadRequest("An error occured, please contact admin.");
        }



        [Route("verify")]
        [HttpPost]
        public async Task <IActionResult> Verify(string token)
        {
            var acc = await _accountRepository.Verify(token);
            if (acc != null)
            {
                
                return Ok($"User {acc.Email} verified at {acc.VerifiedAt}!");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }






        [Route("forgot-password")]
        [HttpPost]
        public async Task< IActionResult> ForgotPassword(string email)
        {
            var mail = await _accountRepository.ForgotPassword(email);
            if(mail != null)
            {
                return Ok("You may now reset your password.");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }




        [Route("reset-password")]
        [HttpPost]
        public async Task <IActionResult> ResetPassword(ResetPasswordDto request)
        {
            if (request == null) return BadRequest();

            if (await _accountRepository.ResetPassword(request))
            {
                return Ok("Password Successfully Reset.");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }

    }
}