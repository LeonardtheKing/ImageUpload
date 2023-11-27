using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

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
            var member = await _context.Users
                        .Where(x => x.UserName == username)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
            return member;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            //   .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            //   .AsNoTracking()
            //   .AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            if (userParams.OrderBy == "created")
            {
                query = query.OrderByDescending(u => u.Created);
            }
            else
            {
                query = query.OrderByDescending(u => u.LastActive);
            }
             return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
            userParams.PageNumber, userParams.PageSize);

            // IQueryable<AppUser> orderedQuery;

            // switch (userParams.OrderBy)
            // {
            //     case "created":
            //         orderedQuery = query.OrderByDescending(u => u.Created);
            //         break;

            //     default:
            //         orderedQuery = query.OrderByDescending(u => u.LastActive);
            //         break;
            // }

            ///Switch statement in C# 8
            // query=userParams.OrderBy switch
            // {
            //     "created" => query.OrderByDescending(u=>u.Created),
            //     _=>query.OrderByDescending(u=>u.LastActive)
            // };
           

        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user;
            // var member = await _context.AppUsers
            //             .Where(x => x.Id == userId)
            //             .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            //             .SingleOrDefaultAsync();
            // return member;
        }


        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {

            var singleUserName = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
            // var anyUserWithTheUserName = await _context.AppUsers.FirstOrDefaultAsync(x=>x.UserName==username);
            return singleUserName;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var appUsers = await _context.Users
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