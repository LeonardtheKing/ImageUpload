using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.OpenApi.Any;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var member = await _context.AppUsers
                        .Where(x => x.UserName == username)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
            return member;
        }

        // public Task<IEnumerable<MemberDto>> GetMembersAsync()
        // {
        //     var members = 
        // }

        // public async Task<AppUser> GetUserByIdAsync(Guid id)
        // {
        // //    return await _context.AppUsers.FindAsync(id);
        // var getUserById = await  _context.AppUsers.FindAsync(id).Include()
        // }

        
        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
            // Use Include to join the AppUsers table with the Photos table
            var userWithPhotos = await _context.AppUsers
                .Where(user => user.Id == id)
                .Include(user => user.Photos) // Assuming there's a navigation property in AppUser for Photos
                .Select(user => new AppUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender,
                    Introduction = user.Introduction,
                    Interests = user.Interests,
                    Country = user.Country,
                    LookingFor = user.LookingFor,
                    City = user.City,
                    Photos = user.Photos.Select(photo => new Photo
                    {
                        // Include the properties you want from the Photo entity
                        Id = photo.Id,
                        Url = photo.Url,
                        IsMain = photo.IsMain,
                        PublicId = photo.PublicId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return userWithPhotos;
        }
        
        

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {

            var singleUserName = await _context.AppUsers
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
            // var anyUserWithTheUserName = await _context.AppUsers.FirstOrDefaultAsync(x=>x.UserName==username);
            return singleUserName;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var appUsers = await _context.AppUsers
            .Include(x => x.Photos).ToListAsync();
            return appUsers;
        }

        public async Task<bool> SaveAllAsync()
        {
            var checkIfChangesHasBeenSaved = await _context.SaveChangesAsync() > 0;
            return checkIfChangesHasBeenSaved;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}