using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>>GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            if(users==null){
                BadRequest();
            }
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
        }

         [Authorize]   
         [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>>GetUsers(Guid id)
        {
            var singleUser = await _userRepository.GetUserByIdAsync(id);
            var singleUserToReturn =_mapper.Map<MemberDto>(singleUser);
            
            return Ok(singleUserToReturn);
        }

        [HttpGet("username")]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>>GetUserByUsername(string username)
        {
            var singleUserName = await _userRepository.GetUserByUsernameAsync(username);
            if(singleUserName==null){
                BadRequest();
            }
            var singleUserToReturn= _mapper.Map<MemberDto>(singleUserName);
            return Ok(singleUserToReturn);
        }
    }
}