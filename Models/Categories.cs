using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    [Table("Category")]
    public class Categories
    {
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
