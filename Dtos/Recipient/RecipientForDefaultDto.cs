using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Recipient
{
    public class RecipientForDefaultDto
    {
        public int RecipientID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public bool Default { get; set; }
        public int? Fee { get; set; }
    }
}
