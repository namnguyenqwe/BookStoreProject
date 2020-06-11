using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BookStoreProject.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using BookStoreProject.Models;
using BookStoreProject.Commons;
using Microsoft.AspNetCore.Http;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController:ControllerBase
    {
        private IUserService _userService;
        private IBaseUrlHelper _baseUrlHelper;
        private IImageFileService _imageFileService;

        public UsersController(IUserService userService,IBaseUrlHelper baseUrlHelper,IImageFileService imageFileService)
        {
            _userService = userService;
            _baseUrlHelper = baseUrlHelper;
            _imageFileService = imageFileService;
        }

        [HttpGet]
        public IActionResult GetUserProfile()
        {
            string userId = GetUserId();
            if(userId == null)
            {
                return Unauthorized();
            }
            var user = _userService.GetSingleByCondition(s => s.Id == userId, null);
          
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                
                return Ok(user);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUserProfile(string id)
        {
            var user = _userService.GetSingleByCondition(s => s.Id == id, null);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        public IActionResult UpdateUserProfile(ProfileViewModel profile)
        {
            string userId = GetUserId();
            if(userId == "error")
            {
                return Unauthorized();
            }
            var user = _userService.GetSingleByCondition(s => s.Id == userId, null);

            user.FullName = profile.FullName;
            _userService.Update(user);
            _userService.SaveChanges();
            return Ok(user);

        }

        [HttpGet]
        [Route("SearchUsersByName")]
        public IActionResult GetUserByName(string name,int index,int size =15)
        {
            var users = _userService.GetMultiPaging(s => s.FullName.Contains(name), index, size, null);
            return Ok(users);
        }

        [HttpPost]
        [Route("Avatar")]
        public async Task<IActionResult> PostUserAvatar([FromForm]IFormFile file)
        {
            string userId = GetUserId();
            if(userId == "error" )
            {
                return Unauthorized();
            }
            var imageName = await _imageFileService.UploadImage(file);
            if(!"failed".Equals(imageName))
            {
                var user = _userService.GetSingleByCondition(s => s.Id == userId, null);
                user.AvatarLink = _baseUrlHelper.GetBaseUrl() + "/Image/" + imageName;
                _userService.Update(user);
                _userService.SaveChanges();
                return Ok(user);
            }
            else
            {
                return BadRequest(new { message = "Invalid image type !" });
            }
        }

        [NonAction]
        public string GetUserId()
        {
            string userId;
            try
            {
                userId = User.Claims.First(c => c.Type == "UserID").Value;
            }
            catch
            {
                return "error";
            }
            return userId;
        }

        

    }
}
