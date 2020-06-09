using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForUserRelatedListDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public string ImageLink { get; set; }
        public decimal? Price { get; set; }
    }
}
