using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Admin
{
    public class UserForListDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public string AvatarLink { get; set; }
        public DateTime? AccountCreateDate { get; set; }
    }
}
