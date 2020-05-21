using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    [Table("City")]
    public class City
    {
        public string CityID { get; set; }
        public string city { get; set; }
        [StringLength(30)]
        public string? type { get; set; }

        public ICollection<Recipient> Recipients { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
