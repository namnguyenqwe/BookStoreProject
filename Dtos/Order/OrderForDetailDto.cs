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
        public string CouponID { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
