using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Coupon
{
    public class CouponForListDto
    {
        public string CouponID { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityUsed { get; set; }
        public string Status { get; set; }
    }
}
