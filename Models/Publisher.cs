using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    [Table("Publisher")]
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string? publisher { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
