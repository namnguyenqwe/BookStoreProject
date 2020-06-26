using System;
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
        public IActionResult test(ApplicationUser User,string role)
        {
            BookStoreDbContext db = new BookStoreDbContext();
            var listUser = _context.Roles.ToList();
            //var user = _userManager.GetUserAsync(User);

            var users = _context.ApplicationUsers.Include(x => x.UserRoles).ThenInclude(x => x.RoleId);

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
                return Ok(user);
            }
        }

        
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
            user.AvatarLink = profile.AvatarLink;
            _userService.Update(user);
            _userService.SaveChanges();
            return Ok(user);

        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                AvatarLink = model.AvatarLink,
                Status = model.Status,
            };
            applicationUser.AccountCreateDate = DateTime.Now;
            
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "User");
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

        [HttpPost]
        [Route("AddAdmin")]
        public async Task<IActionResult> AddAdmin(UserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                AvatarLink = model.AvatarLink,
                Status = model.Status,
            };
            applicationUser.AccountCreateDate = DateTime.Now;

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "Admin");
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

        [HttpPost]
        [Route("AddManager")]
        public async Task<IActionResult> AddManager(UserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                AvatarLink = model.AvatarLink,
                Status = model.Status,
            };
            applicationUser.AccountCreateDate = DateTime.Now;

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "Manager");
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

        [HttpPut]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id,ProfileViewModel profile)
        {
            var user =  _userService.GetSingleByCondition(s=> s.Id == id,null);

            user.FullName = profile.FullName;
            user.Email = profile.Email;
            user.UserName = profile.Email;
            user.Status = profile.Status;
            user.AvatarLink = profile.AvatarLink;

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
