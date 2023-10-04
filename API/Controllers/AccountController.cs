using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using API.DTOs;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }      


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName=registerDto.UserName.ToLower(),
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt=hmac.Key
            };
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            var userDTO = new UserDto{
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
            return userDTO;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _context.AppUsers
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.UserName.ToLower());
            if(user==null)
            {
                return Unauthorized("Invalid username");  
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<ComputeHash.Length;i++){
                if(ComputeHash[i] != user.PasswordHash[i]){
                    return Unauthorized("Invalid Password");
                }
            }
            var userDTO = new UserDto{
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
            return userDTO;
            


        }

        private async Task<bool>UserExists(string username)
        {
            return await _context.AppUsers.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}
