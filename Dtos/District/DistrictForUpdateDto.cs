using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.District
{
    public class DistrictForUpdateDto
    {
        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? Fee { get; set; }
    }
}
