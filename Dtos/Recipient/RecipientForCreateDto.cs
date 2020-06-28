using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Recipient
{
    public class RecipientForCreateDto
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address { get; set; }
        public string CityID { get; set; }
        public string DistrictID { get; set; }
        public bool Default { get; set; } = false;
        public bool Status { get; } = true;
    }
}
