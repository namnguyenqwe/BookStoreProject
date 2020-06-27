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
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace BookStoreProject.Services
{
    public interface IOrdersService
    {
        Task<bool> DeleteOrderAsync(int orderId);
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task<bool> CreateOrderAsync(Orders orderCreate);
        Task<bool> UpdateOrderAsync(Orders order);
        Task<Orders> GetOrderByIdForUserAsync(int orderId);
        IEnumerable<Orders> GetOders(string keyword);
        IEnumerable<OrderForListDto> GetOrdersPerPage(IEnumerable<OrderForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId");
        Task<IEnumerable<Orders>> GetOrder(string applicationUserId);
        Task<decimal?> GetPriceTotal(string from, string to);
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
            return await _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Coupon)
                                           .Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City)
                                           .Include(x => x.OrderItems).ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.OrderID == orderId);
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
            if (criteria.Equals("address"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Address).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Address).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("shippingfee"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.ShippingFee).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.ShippingFee).Skip((page - 1) * pageSize).Take(pageSize);
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
            if (criteria.Equals("email"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return null;
        }

        public IEnumerable<Orders> GetOders(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City)
                    .Where(x =>
                        x.ApplicationUser.Name.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Recipient.Name.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Orders.Include(x => x.ApplicationUser).Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City).AsEnumerable();
        }
        public async Task<Orders> GetOrderByIdForUserAsync(int orderId)
        {
            return await _dbContext.Orders.Include(x => x.Coupon)
                          .Include(x => x.ApplicationUser)
                          .Include(x => x.OrderItems).ThenInclude(x => x.Book)
                          .Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City)
                          .FirstOrDefaultAsync(x => x.OrderID == orderId);
        }

        public async Task<IEnumerable<Orders>> GetOrder(string applicationUserId)
        {
            return await _dbContext.Orders.Include(x => x.Coupon)
                .Include(x => x.ApplicationUser).Include(x => x.OrderItems).ThenInclude(x => x.Book)
                .Include(x => x.Recipient).ThenInclude(x => x.District).ThenInclude(x => x.City)
                .Where(x => x.ApplicationUserID == applicationUserId).ToListAsync();
        }

        public async Task<decimal?> GetPriceTotal(string from, string to)
        {
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var dateStarted = DateTime.ParseExact(from, "d/M/yyyy",
                     CultureInfo.CreateSpecificCulture("fr-FR"));
                var dateEnded = DateTime.ParseExact(to, "d/M/yyyy",
                      CultureInfo.CreateSpecificCulture("fr-FR"));
                var orders = await _dbContext.Orders.Include(x => x.OrderItems).Include(x => x.Coupon)
                        .Where(x => x.Date >= dateStarted
                                    && x.Date <= dateEnded
                                    && x.Status.ToLower() == "Đã hoàn thành".ToLower())
                        .Select(x => x.OrderItems.Sum(x => x.Price) * (100 - (x.Coupon != null ? x.Coupon.Discount : 0) / 100)).ToListAsync();
                return orders.Any() ? orders.Sum(x => x) : 0;
            }    
            var ordersAll = await _dbContext.Orders.Include(x => x.OrderItems).Include(x => x.Coupon)
                        .Where(x => x.Status.ToLower() == "Đã hoàn thành".ToLower())
                        .Select(x => x.OrderItems.Sum(x => x.Price) * (100 - (x.Coupon != null ? x.Coupon.Discount : 0) / 100)).ToListAsync();
            return ordersAll.Any() ? ordersAll.Sum(x => x) : 0;
        }
    }
}
