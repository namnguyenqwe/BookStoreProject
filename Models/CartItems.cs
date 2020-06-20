using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    [Table("CartItems")]
    public class CartItems
    {
        public string ApplicationUserId { get; set; }
        public int BookID { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Book Book { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
