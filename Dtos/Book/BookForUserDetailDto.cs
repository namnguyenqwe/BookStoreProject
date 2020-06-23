using BookStoreProject.Dtos.Review;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForUserDetailDto
    {
        public int BookID { get; set; }
        public string NameBook { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Category { get; set; }
        public string publisher { get; set; }
        public string Author { get; set; }

        //[RegularExpression(@"^ *?\d*\.?\d+ *?x *?\d*\.?\d+ *?cm *?$")]
        public string Dimensions { get; set; }
        public string Format { get; set; }
        public DateTime? Date { get; set; }

        //[Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? NumberOfPage { get; set; }
        public string Information { get; set; }

        //[Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? OriginalPrice { get; set; }

        //[Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? Price { get; set; }
        public int RemainQuantity { get; set; }
        public string ImageLink { get; set; }
        public ICollection<ReviewForUserListDto> Reviews { get; set; }
        public ICollection<BookForUserRelatedListDto> RelatedBooks { get; set; }


    }
}
