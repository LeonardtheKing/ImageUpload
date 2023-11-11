using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(Guid SourceUserId,Guid LikedUserId);
        Task<AppUser> GetUserWithLikes(Guid sourceUserId);
        Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate,Guid userId);


    }
}