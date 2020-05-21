using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public string ApplicationUserID { get; set; }
        public int RecipientID { get; set; }
        public DateTime? Date { get; set; }
        public string CouponID { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Recipient Recipient { get; set; }
        public Coupon Coupon { get; set; }
    }
}
