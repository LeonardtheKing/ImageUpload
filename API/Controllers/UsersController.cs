using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>>GetUsers()
        {
            var users = await _context.AppUsers.ToListAsync();
            return users;
        }

         [Authorize]   
         [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>>GetUsers(int id)
        {
            var users = await _context.AppUsers.FindAsync(id);
            return users;
        }
    }
}