using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [ForeignKey("BookId")]
        public int BookID { get; set; }

        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }

        public int? Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public Book Book { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
