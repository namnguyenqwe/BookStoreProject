using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    public class Subcriber
    {
        [Key]
        public int SubcriberId { get; set; }

        [StringLength(20)]
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
