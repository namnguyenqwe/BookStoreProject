using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Models
{
    public enum ContactStatus
    {
        [Description("Chưa xử lí")]
        NOPROCESS,
        [Description("Đang xử lí")]
        PROCESSING,
        [Description("Đã xử lí")]
        PROCESSED
    }
    [Table("Contact")]
    public class Contact
    {
        public int ContactID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
        public ContactStatus Status { get; set; }
    }
}
