﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using BookStoreProject.Models;
using BookStoreProject.Services;
using BookStoreProject.ViewModels;
using BookStoreProject.Commons;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUserService _userService;
        private IBaseUrlHelper _baseUrlHelper;
        private IImageFileService _imageFileService;
        private readonly BookStoreDbContext _context;
        private BookStoreDbContext db = new BookStoreDbContext();
        public AdminsController(UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<IdentityRole> roleManager,
             BookStoreDbContext context,IBaseUrlHelper baseUrlHelper, IImageFileService imageFileService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
            _baseUrlHelper = baseUrlHelper;
            _imageFileService = imageFileService;
            this._context = context;
        }
        [HttpGet]
        [Route("test")]
        public IActionResult test()
        {
            BookStoreDbContext db = new BookStoreDbContext();
            var listUser = _context.UserRoles.ToList();
            return Ok(listUser);
        }

        [HttpGet]
        public IActionResult GetUserProfile()
        {
            string userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = _userService.GetSingleByCondition(s => s.Id == userId, null);
           
            if (user == null)
            {
                return NotFound();
            }
            else
            {

                return Ok(user);
            }
        }

        [HttpGet]
        [Route("ListUser")]
        public IActionResult GetListUser(int index, int size)
        {
            var users = _userService.GetMultiPaging(s => s.IsDeleted !=true, index, size = 10);
            return Ok(users);
        }

        [HttpGet]
        [Route("SearchUsersByName")]
        public IActionResult GetUserByName(string name, int index, int size = 15)
        {
            var users = _userService.GetMultiPaging(s => s.FullName.Contains(name), index, size, null);
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUserProfile(string id)
        {
            var user = _userService.GetSingleByCondition(s => s.Id == id && s.IsDeleted != true, null);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut]
        public IActionResult UpdateUserProfile(ProfileViewModel profile)
        {
            string userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var user = _userService.GetSingleByCondition(s => s.Id == userId, null);

            user.FullName = profile.FullName;
            _userService.Update(user);
            _userService.SaveChanges();
            return Ok(user);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _userService.GetSingleByCondition(s => s.Id == id,null);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Delete(user);
            _userService.SaveChanges();
            return Ok();

        }
        [HttpPost]
        [Route("Avatar")]
        public async Task<IActionResult> PostUserAvatar([FromForm]IFormFile file)
        {
            string userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var imageName = await _imageFileService.UploadImage(file);
            if (!"failed".Equals(imageName))
            {
                var user = _userService.GetSingleByCondition(s => s.Id == userId, null);
                user.AvatarLink = _baseUrlHelper.GetBaseUrl() + "/Images/" + imageName;
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
