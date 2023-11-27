using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _tokenService = tokenService;
            _mapper = mapper;
        }      


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }
            var user = _mapper.Map<AppUser>(registerDto);
                       
            
                user.UserName=registerDto.UserName.ToLower();
                var result = await _userManager.CreateAsync(user,registerDto.Password);
                if(!result.Succeeded) return BadRequest(result.Errors);
            
         
            var userDTO = new UserDto{
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user),
                KnownAs=user.KnownAs,
                Gender=user.Gender
            };
            return userDTO;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.UserName.ToLower());
            if(user==null)
            {
                return Unauthorized("Invalid username");  
            }
            var result = await _signInManager
                .CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized();
                       
            var userDTO = new UserDto{
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user),
                KnownAs=user.KnownAs,
                Gender=user.Gender
            };
            return userDTO;
            


        }

        private async Task<bool>UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}
