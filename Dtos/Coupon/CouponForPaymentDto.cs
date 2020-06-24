using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Coupon
{
    public class CouponForPaymentDto
    {
        public string CouponID { get; set; }
        public int? Discount { get; set; }
    }
}
