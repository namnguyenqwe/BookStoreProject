using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string AvatarLink { get; set; }

        public bool Status { get; set; }
        public DateTime? AccountCreateDate { get; set; }
        public string Role { get; set; }
    }
}
