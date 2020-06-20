using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.CartItem
{
    public class CartItemForUserListDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public string Author { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
