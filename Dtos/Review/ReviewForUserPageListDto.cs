using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Review
{
    public class ReviewForUserPageListDto
    {
        public float Rating { get; set; }
        public int ReviewCount { get; set; }
        public IEnumerable<ReviewForUserListDto> Reviews { get; set; }
    }
}
