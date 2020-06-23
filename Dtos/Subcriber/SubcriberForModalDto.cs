using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Subcriber
{
    public class SubcriberForModalDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
