using DatingApp.API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;

                //------------------------------------------------------------------------
                ////----
                //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();                
                //configurationBuilder.AddJsonFile("appsettings.json");
                //IConfiguration configuration = configurationBuilder.Build();

                ////-----                        
                //var secretKey = configuration.GetSection("AppSettings:Token").Value;
                ////-----
                ///
                var secretKey = "super secret key";
                //------------------------------------------------------------------------

                CreatePasswordHash("password", secretKey, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
                _context.SaveChanges();
                    
                    
            }

        }

        private void CreatePasswordHash(string password, string secretKey, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            //using (var hmac = new System.Security.Cryptography.HMACSHA512())                
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

    }
}
