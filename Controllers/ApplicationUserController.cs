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
            var role = new IdentityRole();
            role.Name = "User";
            await _roleManager.CreateAsync(role);

            var role2 = new IdentityRole();
            role2.Name = "Admin";
            await _roleManager.CreateAsync(role2);

            var role3 = new IdentityRole();
            role3.Name = "Manager";
            await _roleManager.CreateAsync(role3);



            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                AvatarLink = _baseUrlHelper.GetBaseUrl() + "/Images/defaultAvatar.png",
            };
            applicationUser.AccountCreateDate = DateTime.Now;

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "User");
                }
                return Ok(result);
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
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.PassWord))
            {
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Name",user.UserName.ToString()),
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())                       
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10000),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token, user });

            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect!" });
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
                return BadRequest();
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
    }
}
