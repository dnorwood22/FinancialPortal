using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.CodeFirst
{
    public class Category
    {
        public Category()
        {
            Transactions = new HashSet<AccountTransaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
       

        public virtual ICollection<AccountTransaction> Transactions { get; set; }
    }
}