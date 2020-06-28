using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreProject.Models
{   
    [Table("Recipient")]
    public class Recipient
    {
        public int RecipientID { get; set; }
       // public string ApplicationUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public string DistrictID { get; set; }
        public bool Default { get; set; } 
        public bool Status { get; set; } = true;
        public ICollection<Orders> Orders { get; set; }
        public City City { get; set; }
        public District District { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
    }
}
