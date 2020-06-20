using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Review
{
    public class ReviewForListDto
    {
        public int ReviewId { get; set; }
        public int BookID { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string NameBook { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
