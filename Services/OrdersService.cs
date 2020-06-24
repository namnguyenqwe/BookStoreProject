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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreProject.Services
{
    public interface IOrdersService 
    {
        Task<bool> DeleteOrderAsync(int orderId);
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task<bool> CreateOrderAsync(Orders orderCreate);
        Task<bool> UpdateOrderAsync(Orders order);
        IEnumerable<Orders> GetOders(string keyword);
        IEnumerable<OrderForListDto> GetOrdersPerPage(IEnumerable<OrderForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId");
        Task<IEnumerable<Orders>> GetOrder(string applicationUserId);
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

        public async Task<bool> CreateOrderAsync(Orders orderCreate)
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
            //return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId);
            return await _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.OrderItems).ThenInclude(x => x.Book).Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City).FirstOrDefaultAsync(x => x.OrderID == orderId);
        }

        public async Task<bool> UpdateOrderAsync(Orders order)
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

        public IEnumerable<OrderForListDto> GetOrdersPerPage(IEnumerable<OrderForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId")
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
            return null;
        }

        public IEnumerable<Orders> GetOders(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient).ThenInclude(x=>x.District).ThenInclude(x=>x.City)
                    .Where(x =>
                        x.ApplicationUser.FullName.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Recipient.Name.ToUpper().Contains(keyword.ToUpper()))                    
                    .AsEnumerable();
            }
            return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient).ThenInclude(x=>x.District).ThenInclude(x=>x.City).AsEnumerable();
        }

        public async Task<IEnumerable<Orders>> GetOrder(string applicationUserId)
        {
            return await _dbContext.Orders.Include(x => x.ApplicationUser).Where(x => x.ApplicationUserID == applicationUserId).ToListAsync();
        }
    }
}
