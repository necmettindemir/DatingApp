using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        //public async Task<User> Login(string username, string password)
        public async Task<User> Login(string username, string password, string secretKey)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            //var user = await _context.Users
            //                .Where(x => x.Username == username)
            //                //.OrderBy(YOURCRITERIA)
            //                .Take(1)
            //                //.Select(x => x.Username)
            //                .ToListAsync();

            if (user == null)
                return null;


            //if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            if (!VerifyPasswordHash(password, secretKey, user.PasswordHash, user.PasswordSalt))
                return null;
            

            return user;

        }

        // private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        private bool VerifyPasswordHash(string password, string secretKey, byte[] passwordHash, byte[] passwordSalt)
        {
            //---
            //using (var hmac = new System.Security.Cryptography.HMACSHA512())
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
               var computedSalt = hmac.Key;
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }

            }

            return true;

        }

        public async Task<User> Register(User user, string password, string secretKey)
        //public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            //CreatePasswordHash(password, out passwordHash, out passwordSalt);
            CreatePasswordHash(password, secretKey, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        //private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        private void CreatePasswordHash(string password, string secretKey, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            //using (var hmac = new System.Security.Cryptography.HMACSHA512())                
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
           
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }


    }
}
