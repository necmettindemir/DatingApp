﻿using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        //public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        //public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<PhotosForDetailedDto> Photos { get; set; }

        public string PhotoUrl { get; set; }

    }
}
