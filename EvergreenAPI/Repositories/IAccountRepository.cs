using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IAccountRepository
    {
        Account Login(LoginDto account);
        Task<bool> Register(Account accountDto);
        Task<Account> Verify(string token);
        Task<Account> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordDto request);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
