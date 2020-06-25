using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookStoreProject.Dtos.Order
{
    public class OrderForUserDetailDto
    {
        public int OrderID { get; set; }
        public string NameOfUser { get; set; }
        public string NameOfRecipient { get; set; }
        public DateTime? Date { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string? Status { get; set; }
        public string? Note { get; set; }

        public string[] ListBook { get; set; }
        public decimal TamTinh { get; set; }



        public string CouponID { get; set; }
        public decimal? ShippingFee { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int Discount { get; set; }
        public decimal TamtinhShippingFee { get; set; }
        
        public decimal Total { get; set; }
    }
}
