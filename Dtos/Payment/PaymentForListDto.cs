using BookStoreProject.Dtos.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Payment
{
    public class PaymentForListDto
    {
        public IEnumerable<CartItemForPaymentListDto> CartItems { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
