﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    public class OrderItems
    {
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Book Book { get; set; }
        public virtual Orders Order { get; set; }
    }
}
