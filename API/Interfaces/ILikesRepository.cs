namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int SourceUserId,int LikedUserId);
        Task<AppUser> GetUserWithLikes(int sourceUserId);
        Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate,int userId);


    }
}