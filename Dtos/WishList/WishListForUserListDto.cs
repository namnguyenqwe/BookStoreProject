using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.WishList
{
    public class WishListForUserListDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public string ImageLink { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
    }
}
