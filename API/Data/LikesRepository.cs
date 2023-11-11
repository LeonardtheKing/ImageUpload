using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(Guid SourceUserId, Guid LikedUserId)
        {
            return await _context.Likes.FindAsync(SourceUserId,LikedUserId);
        }

        public async Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, Guid userId)
        {
            var users = _context.AppUsers.OrderBy(u=>u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(predicate == "liked")
            {
                likes=likes.Where(like=>like.SourceUserId==userId);
                users = likes.Select(like=>like.LikedUser);
            }

            if(predicate == "likedBy")
            {
                likes=likes.Where(like=>like.LikedUserId==userId);
                users = likes.Select(like=>like.SourceUser);
            }

            return await users.Select(user=>new LikeDTO
            {
                Username=user.UserName,
                KnownAs=user.KnownAs,
                Age=user.DateOfBirth.CalculateAge(),
                PhotoUrl=user.Photos.FirstOrDefault(p=>p.IsMain).Url,
                City=user.City,
                Id=user.Id

            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(Guid userId)
        {
            return await _context.AppUsers
                 .Include(x=>x.LikedUsers)
                 .FirstOrDefaultAsync(x=>x.Id==userId);
        }
    }
}