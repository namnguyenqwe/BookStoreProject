using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Category
{
    public class CategoryForUserListDto
    {
        
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string ImageLink { get; set; }
    }
}
