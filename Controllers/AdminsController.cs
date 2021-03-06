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
using AutoMapper;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Helpers;
using BookStoreProject.Dtos.ApplicationUser;
using System.Globalization;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Customer manager,Book manager")]
    
    public class AdminsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUserService _userService;
        private IBaseUrlHelper _baseUrlHelper;
        private IImageFileService _imageFileService;
        private readonly BookStoreDbContext _context;
        private BookStoreDbContext db = new BookStoreDbContext();
        private IAdminService _adminService;
        private readonly IMapper _mapper;
        public AdminsController(UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<IdentityRole> roleManager,
             BookStoreDbContext context,IBaseUrlHelper baseUrlHelper, IImageFileService imageFileService,
             IAdminService adminService,IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
            _baseUrlHelper = baseUrlHelper;
            _imageFileService = imageFileService;
            _adminService = adminService;
            _mapper = mapper;
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
        public async Task<IActionResult> GetUserProfile()
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
                var userForReturn = _mapper.Map<ApplicationUserForProfileDto>(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                userForReturn.Role = userRoles.FirstOrDefault();
                userForReturn.Permissions = await _adminService.GetPermissions(userForReturn.Role);
                return Ok(userForReturn);
            }
        }
        [Authorize(Policy = "USER")]
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                AvatarLink = model.AvatarLink,
                Status = model.Status,
            };
            applicationUser.AccountCreateDate = DateTime.Now;

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, model.Role);
                    return Ok(result);
                }
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        [Authorize(Policy = "USER")]
        [HttpPut]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id, ProfileViewModel profile)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Name = profile.Name;
            user.Email = profile.Email;
            user.UserName = profile.Email;
            user.Status = profile.Status;
            user.AvatarLink = profile.AvatarLink;
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRoleAsync(user, profile.Role);
            await _userManager.UpdateAsync(user);
            return Ok(user);

          
        }
        [Authorize(Policy = "USER")]
        [HttpGet]
        [Route("ListUser")]
        public IActionResult GetListUser(string keyword,int page = 1, int pageSize = 10, int sort = 0, string criteria = "Id")
        {
            try
            {
                
                var list = _adminService.GetUsers(keyword);
                var listforDto = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserForListDto>>(list);
                int totalCount = list.Count();

                var response = _adminService.GetUsersPerPage(listforDto, page, pageSize, sort, criteria);

                var paginationSet = new PaginationSet<UserForListDto>()
                {
                    Items = response,
                    Total = totalCount,
                };

                return Ok(paginationSet);
                
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
                
        }
        [Authorize(Policy = "USER")]
        [HttpGet]
        [Route("SearchUsersByName")]
        public IActionResult GetUserByName(string name, int index, int size = 15)
        {
            var users = _userService.GetMultiPaging(s => s.Name.Contains(name), index, size, null);
            return Ok(users);
        }
        [Authorize(Policy = "USER")]
        [HttpGet]
        [Route("SearchUsersByEmail")]
        public IActionResult GetUserByEmail(string email, int index, int size = 15)
        {
            var users = _userService.GetMultiPaging(s => s.Email.Contains(email), index, size, null);
            return Ok(users);
        }

        /*[HttpGet]        
        [Route("SearchUsersByStatus")]
        public IActionResult GetUserByStatus(string status, int index, int size = 15)
        {
            var users = _userService.GetMultiPaging(s => s.Status.Equals(status), index, size, null);
            return Ok(users);
        }*/
        [Authorize(Policy = "USER")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var user = _userService.GetSingleByCondition(s => s.Id == id && s.IsDeleted != true, null);
            if (user == null)
            {
                return NotFound();
            }
            var userForReturn = _mapper.Map<ApplicationUserForProfileDto>(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            userForReturn.Role = userRoles.FirstOrDefault();
            return Ok(userForReturn);
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

            user.Name = profile.Name;
            user.AvatarLink = profile.AvatarLink;
            _userService.Update(user);
            _userService.SaveChanges();
            return Ok(user);

        }
        [Authorize(Policy = "USER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(id);
            }
            var result = await _adminService.DeleteUserAsync(user.Id);
            if(!result)
            {
                return BadRequest(new { message = "Có lỗi trong quá trình xóa dữ liệu" });
            }
            return Ok();

        }
        /*[HttpPost]
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
        }*/
        [Authorize(Policy = "USER")]
        [HttpGet]
        [Route("GetRole")]
        public IActionResult GetListRole()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }
        [HttpGet("authorization")]
        public async Task<IActionResult> GetAuthorization()
        {
            try
            {
                var listForReturn = await _adminService.GetAuthorization();
                return Ok(new { data = listForReturn });
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
       /* [HttpGet("statistic/all")]
        public IActionResult GetUserCount(string from, string to)
        {
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var dateStarted = DateTime.ParseExact(from, "d",
                 CultureInfo.CreateSpecificCulture("fr-FR"));
                var dateEnded = DateTime.ParseExact(from, "d",
                      CultureInfo.CreateSpecificCulture("fr-FR"));
                /* var countFromTo = _userManager.GetUsersInRoleAsync("User").GetAwaiter().GetResult()
                                 .Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.AccountCreateDate) >= dateStarted
                                 && System.Data.Entity.DbFunctions.TruncateTime(x.AccountCreateDate) <= dateEnded).Count();
                var users = from user in _context.ApplicationUsers
                            join userrole in _context.UserRoles on user.Id equals userrole.UserId
                            join role in _context.Roles on userrole.RoleId equals role.Id
                            where (role.Name == "User"
                                 &&  System.Data.Entity.DbFunctions.TruncateTime( user.AccountCreateDate) >= dateStarted
                                 &&  System.Data.Entity.DbFunctions.TruncateTime(user.AccountCreateDate) <= dateEnded)
                            select user;
                return Ok(new { userCount = users.ToList().Count() });
            }
            var countAll = _userManager.GetUsersInRoleAsync("User").Result.Count;
            return Ok(new { userCount = countAll });
        }*/
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
