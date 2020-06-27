using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;

namespace BookStoreProject.Services
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUser> GetUsers(string keyword);
        IEnumerable<UserForListDto> GetUsersPerPage(IEnumerable<UserForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "Id");
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserByIdAsync(string userid);
        Task<IEnumerable<RoleForAuthorizeDetailDto>> GetAuthorization();
        Task<bool> UpdateAuthorization();
        Task<IEnumerable<string>> GetPermissions(string roleName);
    }
    public class AdminService : IAdminService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        private RoleManager<IdentityRole> _roleManager;
        public AdminService(BookStoreDbContext dbContext, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _roleManager = roleManager;
        }


        public IEnumerable<UserForListDto> GetUsersPerPage(IEnumerable<UserForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "Id")
        {
            criteria = criteria.ToLower();

            if(criteria.Equals("id"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else return list.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
            }
            
            if (criteria.Equals("name"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("email"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("accountcreatedate"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.AccountCreateDate).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.AccountCreateDate).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("status"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return null;
        }

        public IEnumerable<ApplicationUser> GetUsers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.ApplicationUsers
                    .Where(x =>
                    x.Name.ToUpper().Contains(keyword.ToUpper()) ||
                    x.Email.ToUpper().Contains(keyword.ToUpper())).AsEnumerable();
                     
            }
            return _dbContext.ApplicationUsers.AsEnumerable();
        }

        public async Task<IEnumerable<RoleForAuthorizeDetailDto>> GetAuthorization()
        {
            List<RoleForAuthorizeDetailDto> listForReturn = new List<RoleForAuthorizeDetailDto>();
            var roles = await _dbContext.Roles.ToListAsync();
            foreach (var role in roles)
            {
                var permissionClaims = await  _dbContext.RoleClaims.Where(x => x.RoleId == role.Id && x.ClaimType == "PERMISSION")
                                                .Select(x => x.ClaimValue).ToListAsync();
                listForReturn.Add(new RoleForAuthorizeDetailDto()
                {
                    Role = role.Name,
                    Permission = string.Join(", ", permissionClaims)
                }) ;
            }
            return listForReturn;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _dbContext.ApplicationUsers.Include(x => x.WishLists)
                            .Include(x => x.CartItems)
                            .Include(x => x.Reviews)
                            .Include(x => x.CartItems)
                            .Include(x => x.Orders)
                            .FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                    return false;

                if (user.CartItems.Any())
                {
                    _dbContext.CartItems.RemoveRange(user.CartItems);
                }

                if (user.Reviews.Any())
                {
                    _dbContext.Reviews.RemoveRange(user.Reviews);
                }

                if (user.WishLists.Any())
                {
                    _dbContext.WishLists.RemoveRange(user.WishLists);
                }

                if (user.Reviews.Any())
                {
                    _dbContext.Orders.RemoveRange(user.Orders);
                }
                _dbContext.ApplicationUsers.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdateAuthorization()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetPermissions(string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name.ToUpper() == roleName.ToUpper());
            var permissions = await _dbContext.RoleClaims.Where(x => x.RoleId == role.Id && x.ClaimType == "PERMISSION")
                                                .Select(x => x.ClaimValue).ToListAsync();
            return permissions;
        }
    }
}
