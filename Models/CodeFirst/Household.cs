using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.CodeFirst
{
    public class Household
    {
        public Household()
        {
            Users = new HashSet<ApplicationUser>();
            Accounts = new HashSet<Account>();
            Budgets = new HashSet<Budget>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}