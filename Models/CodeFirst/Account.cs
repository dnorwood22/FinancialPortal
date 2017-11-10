using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.CodeFirst
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<AccountTransaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public int? AccountTypeId { get; set; }
        public int? HouseholdId { get; set; }
        public DateTime? Reconciled { get; set; }
        public string AssignToUserId { get; set; }

        public virtual Household Household { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual ApplicationUser AssignToUser { get; set; }

        public virtual ICollection<AccountTransaction>Transactions { get; set; }

    }
}