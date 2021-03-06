﻿using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthRepository repo, 
            IConfiguration config,
            IMapper mapper            
            )
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }


        [HttpPost("Register")]
        //public async Task<IActionResult> Register(string username, string password)
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            //validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("username already exists!");


            /* 
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            */
            var userToCreate = _mapper.Map<User>(userForRegisterDto);


            var secretKey = _config.GetSection("AppSettings:Token").Value;
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password, secretKey);


            var usertoReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            //return Ok(createdUser);
            return CreatedAtRoute("GetUser", 
                                    new {controller="Users", id= createdUser.Id},
                                    usertoReturn );
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            // throw new Exception("computer says no!");


            try
            {

                var secretKey = _config.GetSection("AppSettings:Token").Value;
                var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password, secretKey);

                if (userFromRepo == null)
                    return Unauthorized();

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var user = _mapper.Map<UserForListDto>(userFromRepo);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    user 
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }// Login


    }
}
