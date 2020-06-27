using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookStoreProject.Helpers;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


namespace BookStoreProject.Services
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUser> GetUsers(string keyword);
        IEnumerable<UserForListDto> GetUsersPerPage(IEnumerable<UserForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "Id");

        Task<bool> DeleteUserAsync(string userId);
    }
    public class AdminService : IAdminService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public AdminService(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
            
            if (criteria.Equals("fullname"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.FullName).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.FullName).Skip((page - 1) * pageSize).Take(pageSize);
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
            return null;
        }

        /*public IEnumerable<ApplicationUser> GetUsers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.ApplicationUsers
                    .Where( delegate (ApplicationUser b)
                    {
                        if (MyConvert.ConvertToUnSign(b.FullName.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    
                        b.FullName.ToUpper().Contains(keyword.ToUpper()) )

                            return true;
                         else
                            return false;
                    })
                    .AsEnumerable();

            }
            return _dbContext.ApplicationUsers.AsEnumerable();
        }*/

        public IEnumerable<ApplicationUser> GetUsers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.ApplicationUsers
                    .Where(x =>
                    x.FullName.ToUpper().Contains(keyword.ToUpper()) ||
                    x.Email.ToUpper().Contains(keyword.ToUpper())).AsEnumerable();
                  
            }
            return _dbContext.ApplicationUsers.AsEnumerable();
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
                    _dbContext.Reviews.RemoveRange(user.Reviews);
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
    }
}
