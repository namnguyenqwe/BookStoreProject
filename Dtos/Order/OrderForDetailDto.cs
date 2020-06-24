using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Order
{
    public class OrderForDetailDto
    {
        public int OrderID { get; set; }
        public string ApplicationUserID { get; set; }
        public int RecipientID { get; set; }
        public DateTime? Date { get; set; }
        public string Email { get; set; }
        public string CouponID { get; set; }
        public string Address { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public string[] NameBook { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Pay { get; set; }
        //public string CouponId { get; set; }
        public int Discount { get; set; }
        public decimal Total1 { get; set; }
        public decimal Total2 { get; set; }
    }
}
