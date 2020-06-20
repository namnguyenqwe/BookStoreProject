using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Category
{
    public class CategoryDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Category { get; set; }
    }
}
