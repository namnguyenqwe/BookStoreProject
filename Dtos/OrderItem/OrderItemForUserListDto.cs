﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.OrderItem
{
    public class OrderItemForUserListDto
    {
        public string NameBook { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
