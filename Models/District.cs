using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    [Table("District")]
    public class District
    {
        public string DistrictID { get; set; }
        public string district { get; set; }
        [StringLength(30)]
        public string type { get; set; }
        public string CityID { get; set; }
        public int? Fee { get; set; }

        public ICollection<Recipient> Recipients { get; set; }
        public City City { get; set; }
    }
}
