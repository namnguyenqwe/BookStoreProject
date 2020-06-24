using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.Infrastructure;
using BookStoreProject.Repositorys;
using BookStoreProject.Dtos.Order;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStoreProject.Services
{
    public interface IOrdersService
    {
        Task<bool> DeleteOrderAsync(int orderId);
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task<bool> CreateBookAsync(Orders orderCreate);
        Task<bool> UpdateBookAsync(Orders order);
        IEnumerable<Orders> GetOders(string keyword);
        IEnumerable<OrderForListDto> GetBooksPerPage(IEnumerable<OrderForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId");
    }
    public class OrdersService:IOrdersService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrdersService(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateBookAsync(Orders orderCreate)
        {
            try
            {

                _dbContext.Orders.Add(orderCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId);
                if (order == null)
                    return false;
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId);
        }

        public async Task<bool> UpdateBookAsync(Orders order)
        {
            try
            {

                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<OrderForListDto> GetBooksPerPage(IEnumerable<OrderForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId")
        {
            criteria = criteria.ToLower();

            if (criteria.Equals("orderid"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (criteria.Equals("name"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.NameOfUser).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.NameOfUser).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("nameofrecipent"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.NameOfRecipent).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.NameOfRecipent).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("coupon"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Coupon).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Coupon).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("phone"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Phone).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Phone).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("date"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Date).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Date).Skip((page - 1) * pageSize).Take(pageSize);
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
            if (criteria.Equals("note"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Note).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Note).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return null;
        }

        public IEnumerable<Orders> GetOders(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient)
                    .Where(x =>
                        x.ApplicationUser.FullName.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Recipient.Name.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient).AsEnumerable();
        }
    }
}
