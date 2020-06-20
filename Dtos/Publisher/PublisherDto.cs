using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Publisher
{
    public class PublisherDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Publisher can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string publisher { get; set; }
    }
}
