using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.CartItem
{
    public class CartItemForPaymentListDto
    {
        public string NameBook { get; set; }
        public string ImageLink { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
