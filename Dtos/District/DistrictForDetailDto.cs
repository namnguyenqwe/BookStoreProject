using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.District
{
    public class DistrictForDetailDto
    {
        public string DistrictID { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public int? Fee { get; set; }
    }
}
