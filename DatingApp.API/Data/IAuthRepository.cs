using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password, string secretKey);

        Task<User> Login(string username, string password, string secretKey);

        Task<bool> UserExists(string username);

    }
}
