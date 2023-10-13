using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public IPhotoService _photoService { get; }

        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            _photoService = photoService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>>GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            if(users==null){
                BadRequest();
            }
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
        }

        //  [Authorize] 
         [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>>GetUsers(Guid id)
        {
            var singleUser = await _userRepository.GetUserByIdAsync(id);
            var singleUserToReturn =_mapper.Map<MemberDto>(singleUser);
            
            return Ok(singleUserToReturn);
        }

        // [HttpGet("{username}", Name="GetUser")]
        [HttpGet]
        [Route("username", Name = "GetUsers")]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>>GetUserByUsername(string username)
        {
            // var singleUserName = await _userRepository.GetUserByUsernameAsync(username);
            var singleMember = await _userRepository.GetMemberAsync(username);
            if(singleMember==null){
                BadRequest();
            }
            // var singleUserToReturn= _mapper.Map<MemberDto>(singleUserName);
            return Ok(singleMember);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDto updateMemberDto)
        {
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(updateMemberDto,user);
            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to update user");
             
        }



        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            // Your logic for setting IsMain and adding the photo to the user's collection
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDto>(photo);
                // return CreatedAtRoute("GetUser",_mapper.Map<PhotoDto>(photo));
                 return CreatedAtRoute("GetUser",new{Username=user.UserName},_mapper.Map<PhotoDto>(photo));

            }

            return BadRequest("Problem adding Photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
public async Task<ActionResult> SetMainPhoto(Guid photoId)
{
    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

    if (user == null)
    {
        return NotFound("User not found");
    }

    var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

    if (photo == null)
    {
        return NotFound("Photo not found");
    }

    if (photo.IsMain)
    {
        return BadRequest("This is already your main photo");
    }

    var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

    if (currentMain != null)
    {
        currentMain.IsMain = false;
    }

    photo.IsMain = true;

    if (await _userRepository.SaveAllAsync())
    {
        return NoContent();
    }

    return BadRequest("Failed to set main photo");
}


        // [HttpPut("set-main-photo/{photoId}")]
        // public async Task<ActionResult> SetMainPhoto(Guid photoId)
        // {
        //     var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        //     // var photo =user.Photos.FirstOrDefault(x=>x.Id==photoId);
        //     var photo =user.Photos.FirstOrDefault(x=>x.Id==photoId);
        //     if(photo.IsMain){
        //         return BadRequest("This is already your main photo");
        //     }
        //     var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);
        //     if(currentMain!=null){
        //         currentMain.IsMain=false;
        //     }
        //     photo.IsMain=true;
        //     if(await _userRepository.SaveAllAsync()){
        //         return NoContent();
        //     }
        //     return BadRequest("Failed to return main Photo");
        // }

    }
}