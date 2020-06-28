using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Recipient
{
    public class RecipientForUpdateDto
    {
        [Required]
        public bool Default { get; set; } 
    }
}
