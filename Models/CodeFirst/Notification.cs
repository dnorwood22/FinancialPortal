using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.CodeFirst
{
    public class Notification
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string NotifyUserId { get; set; }

        public virtual ApplicationUser NotifyUser { get; set; }
        public virtual Account Account { get; set; }
    }
}