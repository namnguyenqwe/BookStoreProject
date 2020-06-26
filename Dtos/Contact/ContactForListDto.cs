using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Contact
{
    public class ContactForListDto
    {
        public int ContactID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
       // public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
