using AutoMapper;
using BookStoreProject.Dtos.CartItem;
using BookStoreProject.Dtos.Payment;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IPaymentService
    {
        Task<PaymentForListDto> GetPaymentList(string applicationUserId, string[] BookIds);
        Task<bool> SavePaymentBill(Orders orderCreate);
        Task<bool> SaveBillItems(Orders order, string[] bookIds);
    }
    public class PaymentService : IPaymentService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public PaymentService (BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<PaymentForListDto> GetPaymentList(string applicationUserId, string[] bookIds)
        {
            try 
            {
                bookIds = bookIds.Distinct().ToArray();
                List<CartItems> cartItems = new List<CartItems>();
                foreach (var bookId in bookIds)
                {
                    var cartItem = await _dbContext.CartItems.Include(x => x.Book)
                                        .FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.BookID == Convert.ToInt32(bookId));
                    if (cartItem != null)
                        cartItems.Add(cartItem);
                }
                //var cartItems = await _dbContext.CartItems.Include(x => x.Book).Where(x => x.ApplicationUserId == applicationUserId).ToListAsync();
                var cartItemsForReturn = _mapper.Map<IEnumerable<CartItems>, IEnumerable<CartItemForPaymentListDto>>(cartItems);
               /* var cartItemsToRemove =  (await _dbContext.CartItems.ToListAsync()).Except(cartItems).ToList();
                if (cartItemsToRemove.Any())
                {
                    _dbContext.CartItems.RemoveRange(cartItemsToRemove);
                }*/
               // await _dbContext.SaveChangesAsync();
               /* var Total = cartItemsForReturn
                                .Aggregate(new CartItemForPaymentListDto 
                                {
                                    Quantity = 0,
                                    Price = 0,
                                    Weight = 0
                                },
                                (a, b) => new CartItemForPaymentListDto
                                {
                                    Quantity = 1,
                                    Price = a.Price * a.Quantity + b.Price * b.Quantity,
                                    Weight = a.Weight + b.Weight
                                });*/
                return new PaymentForListDto()
                {
                    CartItems = cartItemsForReturn,
                    TotalPrice = cartItemsForReturn.Any() ? cartItemsForReturn.Sum(x => x.Price * x.Quantity) : 0,
                    TotalWeight = cartItemsForReturn.Any() ? cartItemsForReturn.Sum(x => x.Weight * x.Quantity) : 0
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public async Task<bool> SaveBillItems(Orders order, string[] bookIds)
        {
            try
            {
                var orderID = await _dbContext.Orders.Where(x => x.ApplicationUserID == order.ApplicationUserID &&
                            x.CouponID == order.CouponID &&
                            x.Date == order.Date &&
                            x.Note == order.Note &&
                            x.RecipientID == order.RecipientID &&
                            x.ShippingFee == order.ShippingFee)
                            .Select(x => x.OrderID).FirstOrDefaultAsync();
                bookIds = bookIds.Distinct().ToArray();
                List<CartItems> cartItems = new List<CartItems>();
                foreach (var bookId in bookIds)
                {
                    var cartItem = await _dbContext.CartItems.Include(x => x.Book)
                                        .FirstOrDefaultAsync(x => x.ApplicationUserId == order.ApplicationUserID && x.BookID == Convert.ToInt32(bookId));
                    cartItems.Add(cartItem);
                }
                //var cartItems = await _dbContext.CartItems.Include(x => x.Book).Where(x => x.ApplicationUserId == order.ApplicationUserID).ToListAsync();
                var orderItems = cartItems.Select(x => new OrderItems()
                {
                    OrderID = orderID,
                    BookID = x.BookID,
                    Quantity = x.Quantity,
                    Price = x.Quantity * x.Book.Price
                });
                _dbContext.OrderItems.AddRange(orderItems);
                var cartItemsInDB = await _dbContext.CartItems.Include(x => x.Book).Where(x => x.ApplicationUserId == order.ApplicationUserID).ToListAsync();
                if (cartItemsInDB.Any())
                    _dbContext.CartItems.RemoveRange(cartItemsInDB);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SavePaymentBill(Orders orderCreate)
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
    }
}
