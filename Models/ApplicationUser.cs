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
        public string? AvatarLink { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(50)]
        public string? Salt { get; set; }
         
        public DateTime? AccountCreateDate { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<CartItems> CartItems { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Orders> Orders { get; set; }

        //public ICollection<Recipient> Recipients { get; set; }

        /*public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }*/
    }

    /*public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }*/
}
