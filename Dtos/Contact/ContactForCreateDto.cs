using BookStoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Contact
{
    public class ContactForCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Message can not be null or empty")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Message { get; set; }
        public DateTime? Date { get;  } = DateTime.Now;
        public ContactStatus Status { get;  } = ContactStatus.NOPROCESS;
    }
}
