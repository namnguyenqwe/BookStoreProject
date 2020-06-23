using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "NameBook can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NameBook { get; set; }

        public int CategoryID { get; set; }

        public int PublisherID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "NameBook can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Author { get; set; }

        [RegularExpression(@"^ *?\d*\.?\d+ *?x *?\d*\.?\d+ *?cm *?$")]
        public string Dimensions { get; set; }

        public string Format { get; set; }
        public DateTime? Date { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? NumberOfPage { get; set; }

        [StringLength(Int32.MaxValue, MinimumLength = 200)]
        public string Information { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? OriginalPrice { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? Price { get; set; }

        public string ImageLink { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? QuantityIn { get; set; }
        public bool? Status { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public float? Weight { get; set; }
    }
}
