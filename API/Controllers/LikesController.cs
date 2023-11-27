namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController{
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        
        public LikesController(IUserRepository userRepository,ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
            
        }
         
        [HttpPost("{username}")]
        public async Task<ActionResult>AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var LikedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser=await _likesRepository.GetUserWithLikes(sourceUserId);

            if(LikedUser==null){
                return NotFound();
            }

            if(sourceUser.UserName==username)
            {
                return BadRequest("You cannot like yourself");
            }

            var userLike = await _likesRepository.GetUserLike(sourceUserId,LikedUser.Id);

            if(userLike != null) return BadRequest("You already like this user");

             userLike=new UserLike
            {
                SourceUserId=sourceUserId,
                LikedUserId=LikedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if(await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLike(string predicate)
        {
            var users = await _likesRepository.GetUserLikes(predicate,User.GetUserId());
            return Ok(users);
        }
    }
}