using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.CodeFirst
{
    public class AccountTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? AccountId { get; set; }
        public int? TransactionTypeId { get; set; }
        public decimal Amount { get; set; }
        public bool ReconciledStatus { get; set; }
        public bool Voided { get; set; }
        public decimal ReconciledAmount { get; set; }
        public DateTime Created { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ReconciliationDate { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Category Category{ get; set; }
        public virtual Account Account { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}