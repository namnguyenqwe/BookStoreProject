using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    [Table("Coupon")]
    public class Coupon
    {
        public string CouponID { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityUsed { get; set; }
        public string? Status { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
