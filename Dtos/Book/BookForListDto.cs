using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForListDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string publisher { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityIn { get; set; }
        public int? QuantityOut { get; set; }
    }
}
