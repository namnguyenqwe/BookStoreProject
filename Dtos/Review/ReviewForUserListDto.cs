using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Review
{
    public class ReviewForUserListDto
    {
        public int ReviewId { get; set; }
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public bool? isPurchased { get; set; }
        public DateTime Date { get; set; }
    }
}
