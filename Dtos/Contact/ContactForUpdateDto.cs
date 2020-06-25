using BookStoreProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Contact
{
    public class ContactForUpdateDto
    {
        [Range(0,2,ErrorMessage = "Status does'nt exist !")]
        public ContactStatus Status { get; set; }
    }
}
