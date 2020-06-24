using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForUserPopularListDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public string ImageLink { get; set; }
        public int Rating { get; set; }
        public string Author { get; set; }
        public string Information { get; set; }
        public int ReviewCount { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
    }
}
