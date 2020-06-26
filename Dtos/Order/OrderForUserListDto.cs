using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Order
{
    public class OrderForUserListDto
    {
        public int OrderId { get; set; }

        public DateTime Date { get; set; }
        public decimal Tamtinh {get;set;}
        public decimal Shippingfee { get; set; }
        public decimal Discount { get; set; }
         
        public string Status { get; set; }
        public decimal Total { get; set; }
    }
}
