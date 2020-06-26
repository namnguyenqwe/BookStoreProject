using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.ViewModels
{
    public class RolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
