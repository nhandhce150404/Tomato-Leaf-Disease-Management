using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IUserRepository
    {
        bool CreateUser(UserDto user);

        Account GetUser(int id);

        bool DeleteUser(int id);

        bool UpdateUser(Account user, int id);

        ICollection<Account> GetUsers();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        List<Account> Search(string search);

    }
}
