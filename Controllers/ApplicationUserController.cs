using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BookStoreProject.Models;
using BookStoreProject.Services;
using BookStoreProject.Commons;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationSetting _appSettings;
        private IBaseUrlHelper _baseUrlHelper;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSetting> appSettings,IBaseUrlHelper baseUrlHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _baseUrlHelper = baseUrlHelper;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserModel model)
        {
            var applicationUser = new ApplicationUser()
            {              
                Email = model.Email,
                Name = model.Name,
                UserName = model.Email,
                AvatarLink = _baseUrlHelper.GetBaseUrl() + "/Images/defaultAvatar.png",
            };
            applicationUser.AccountCreateDate = DateTime.Now;
            applicationUser.Status = true;

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
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, model.PassWord))
            {
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Name",user.UserName.ToString()),
                        new Claim("Email",user.Email.ToString()),
                        new Claim("UserID",user.Id.ToString()),
                        //new Claim("Status",(user.Status==null?true:user.Status).ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())  
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10000),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });

            }
          
            else
            {

                return BadRequest(new { message = "Email or password is incorrect!" });

            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            string userId;
            try
            {
                userId = User.Claims.First(c => c.Type == "UserID").Value;
            }
            catch
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return BadRequest(new {message = "Old password is incorrect!" });
            }
            else
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }
                return Ok();
            }


        }
        [HttpGet("statistic/all")]
        public IActionResult GetUserCount(string from, string to)
        {
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var dateStarted = DateTime.ParseExact(from, "d/M/yyyy",
                 CultureInfo.CreateSpecificCulture("fr-FR"));
                var dateEnded = DateTime.ParseExact(to, "d/M/yyyy",
                      CultureInfo.CreateSpecificCulture("fr-FR"));
                var countFromTo = _userManager.GetUsersInRoleAsync("User").GetAwaiter().GetResult()
                                .Where(x => x.AccountCreateDate >= dateStarted 
                                && x.AccountCreateDate <= dateEnded) .Count();
                return Ok(new { userCount = countFromTo });
            }    
             var countAll = _userManager.GetUsersInRoleAsync("User").Result.Count;
            return Ok(new { userCount = countAll });
        }
    }
}
