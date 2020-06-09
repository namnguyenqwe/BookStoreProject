using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        [StringLength(200)]
        public string NameBook { get; set; }
        [ForeignKey("CategoryID")]
        public int CategoryID { get; set; }
        [ForeignKey("PublisherID")]
        public int PublisherID { get; set; }
        public string Author { get; set; }
        public string Dimensions { get; set; }

        public string Format { get; set; }
        public DateTime Date { get; set; }
        public int? NumberOfPage { get; set; }

        public string Information { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
        [StringLength(300)]
        public string ImageLink { get; set; }
        public int? QuantityIn { get; set; }
        public int? QuantityOut { get; set; }
        public bool? Status { get; set; }

        public float? Weight { get; set; }
       

        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
        
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual Categories Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }

}
