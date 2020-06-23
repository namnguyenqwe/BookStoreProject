﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Dtos.Admin;
using BookStoreProject.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStoreProject.Services
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUser> GetUsers(string keyword);
        IEnumerable<UserForListDto> GetUsersPerPage(IEnumerable<UserForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "Id");
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

            if(criteria.Equals("Id"))
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
            return null;
        }

        public IEnumerable<ApplicationUser> GetUsers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.ApplicationUsers
                    .Where(x =>
                    x.FullName.ToUpper().Contains(keyword.ToUpper())).AsEnumerable();
                     
            }
            return _dbContext.ApplicationUsers.AsEnumerable();
        }

    }
}