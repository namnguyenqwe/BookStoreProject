using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Recipient
{
    public class RecipientForUserDetailDto
    {
        public int RecipientID { get; set; }
        // public string ApplicationUserID { get; set; }
        public string Name { get; set; }
       // public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public string DistrictID { get; set; }
        public bool Default { get; set; } = false;
    }
}
