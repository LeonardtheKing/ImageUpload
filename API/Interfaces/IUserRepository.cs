using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>>GetUsersAsync();
        // Task<AppUser> GetUserByIdAsync(Guid id);
        Task<AppUser> GetUserByIdAsync(Guid userId);

        
      
     Task<AppUser> GetUserByUsernameAsync(string username);
     Task<PagedList<MemberDto>>GetMembersAsync(UserParams userParams);
     Task<MemberDto>   GetMemberAsync(string username);

    }
}