using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    [Table("WishList")]
   
    public class WishList
    {
        public string ApplicationUserId { get; set; }
        public int BookID { get; set; }
       
       
        
        public Book Book { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
