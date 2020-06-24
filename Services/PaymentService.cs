using AutoMapper;
using BookStoreProject.Dtos.CartItem;
using BookStoreProject.Dtos.Payment;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IPaymentService
    {
        Task<PaymentForListDto> GetPaymentList(string applicationUserId);
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
        public async Task<PaymentForListDto> GetPaymentList(string applicationUserId)
        {
            var cartItems = await _dbContext.CartItems.Include(x => x.Book).Where(x => x.ApplicationUserId == applicationUserId).ToListAsync();
            var cartItemsForReturn = _mapper.Map<IEnumerable<CartItems>,IEnumerable<CartItemForPaymentListDto>>(cartItems);
            var Total = cartItemsForReturn
                            .Aggregate((a, b) => new CartItemForPaymentListDto 
                            { 
                                Quantity = 1,
                                Price = a.Price * a.Quantity + b.Price * b.Quantity ,
                                Weight = a.Weight + b.Weight
                            });
            return new PaymentForListDto()
            {
                CartItems = cartItemsForReturn,
                TotalPrice = Total.Price,
                TotalWeight = Total.Weight
            };
        }
    }
}
