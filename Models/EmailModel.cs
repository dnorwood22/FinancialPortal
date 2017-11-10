using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Email"), EmailAddress]
        public string EmailTo { get; set; }
        public string Body { get; set; }
        public int HouseholdId { get; set; }
    }
}