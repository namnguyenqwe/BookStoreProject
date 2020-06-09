using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Publisher
{
    public class PublisherForListDto
    {
        public int PublisherID { get; set; }
        public string publisher { get; set; }
        public int BookTitleCount { get; set; }
    }
}
