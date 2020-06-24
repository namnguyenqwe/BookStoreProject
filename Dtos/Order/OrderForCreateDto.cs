using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookStoreProject.Dtos.Order
{
    public class OrderForCreateDto
    {
        public string ApplicationUserID { get; set; }
        public int RecipientID { get; set; }
        public DateTime? Date { get; set; }
        public string CouponID { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? ShippingFee { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
