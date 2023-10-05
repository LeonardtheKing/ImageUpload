using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
           return await _context.AppUsers.FindAsync(id);
        }

        public async  Task<AppUser>GetUserByUsernameAsync(string username)
        {
            
             var singleUserName = await _context.AppUsers.SingleOrDefaultAsync(x=>x.UserName==username);
            // var anyUserWithTheUserName = await _context.AppUsers.FirstOrDefaultAsync(x=>x.UserName==username);
            return singleUserName;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var appUsers= await _context.AppUsers.ToListAsync();
            return appUsers;
        }

        public async Task<bool> SaveAllAsync()
        {
            var checkIfChangesHasBeenSaved= await _context.SaveChangesAsync()> 0;
            return checkIfChangesHasBeenSaved;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State=EntityState.Modified;
        }
    }
}