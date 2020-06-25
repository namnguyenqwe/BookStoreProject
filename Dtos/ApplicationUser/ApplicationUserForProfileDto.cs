using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.ApplicationUser
{
    public class ApplicationUserForProfileDto
    {
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarLink { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public DateTime? AccountCreateDate { get; set; }
    }

}
