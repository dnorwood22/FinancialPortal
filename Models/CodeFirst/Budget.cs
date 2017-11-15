using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentDateTime;

namespace FinancialPortal.Models.CodeFirst
{
    public class Budget
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int FrequencyId { get; set; }
        public string Description { get; set; }
        public int HouseholdId { get; set; }
        public decimal Amount { get; set; }

        public virtual Frequency Frequency { get; set; }
        public virtual Household Household { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public decimal? SpentAmount
        {
            get
            {
                if (Household != null && Category != null && Frequency != null)
                {
                    decimal amount = 0;
                    if (Frequency.Name == "Weekly")
                    {
                        var previousSunday = DateTime.Now.Previous(DayOfWeek.Sunday);
                        var nextMonday = DateTime.Now.Next(DayOfWeek.Monday);

                        foreach (var trans in Category.Transactions.Where(t => t.Account.HouseholdId == HouseholdId && t.TransactionDate > previousSunday && t.TransactionDate < nextMonday && t.Voided == false).ToList())
                        {
                            amount += trans.Amount;

                        }
                        return amount;
                    }
                    else if (Frequency.Name == "Monthly")
                    {
                        foreach (var trans in Category.Transactions.Where(t => t.Account.HouseholdId == HouseholdId && t.TransactionDate.Month == DateTime.Now.Month && t.TransactionDate.Year == DateTime.Now.Year && t.Voided == false).ToList())
                        {
                            amount -= trans.Amount;
                        }
                        return amount;
                    }
                    else if (Frequency.Name == "Yearly")
                    {
                        foreach (var trans in Category.Transactions.Where(t => t.Account.HouseholdId == HouseholdId && t.TransactionDate.Year == DateTime.Now.Year && t.Voided == false).ToList())
                        {
                            amount -= trans.Amount;
                        }
                        return amount;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public int? BudgetPercentage
        {
            get
            {
                if (Amount > 0)
                {
                    return Convert.ToInt32((SpentAmount / Amount) * 100);
                }
                else
                {
                    return null;
                }

            }
        }
    }
}