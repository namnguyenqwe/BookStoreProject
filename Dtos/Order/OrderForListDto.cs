using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Order
{
    public class OrderForListDto
    {
        public int Id { get; set; }
        public string NameOfUser { get; set; }

        public string NameOfRecipent { get; set; }

        public string Phone { get; set; }

        public string Coupon { get; set; }

        public DateTime? Date { get; set; }

        public string Address { get; set; }
        public decimal? ShippingFee { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
