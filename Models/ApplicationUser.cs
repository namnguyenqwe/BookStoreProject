using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }

        [StringLength(300)]
        public string AvatarLink { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(50)]

        public string? Salt { get; set; }
         
        public DateTime? AccountCreateDate { get; set; }

        public ICollection<WishList> WishLists { get; set; }
        public ICollection<CartItems> CartItems { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
