using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Payment
{
    public class PaymentForBillDto
    {
        public int RecipientID { get; set; }
        public string CouponID { get; set; }
        public decimal? ShippingFee { get; set; }
        public string Note { get; set; }
    }
}
