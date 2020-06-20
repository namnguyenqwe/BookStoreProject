using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Coupon
{
    public class CouponForModalDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "CouponId can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CouponID { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? Discount { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? Quantity { get; set; }
        public string Status { get; set; }
    }
}
