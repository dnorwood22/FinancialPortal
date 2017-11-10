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
        public decimal? BudgetAmount
        {
            get
            {
                if (Category != null)
                {
                    if(Frequency != null && Frequency.Name == "Weekly")
                    {
                        var previousSunday = DateTime.Now.Previous(DayOfWeek.Sunday);
                        var nextMonday = DateTime.Now.Next(DayOfWeek.Monday);
                        return Category.Transactions.Where(t => t.TransactionDate > previousSunday && t.TransactionDate < nextMonday && t.Voided == false).Sum(t => t.Amount);
                    } 
                    else if (Frequency != null && Frequency.Name == "Monthly")
                    {
                        return Category.Transactions.Where(t => t.TransactionDate.Month == DateTime.Now.Month && t.TransactionDate.Year == DateTime.Now.Year && t.Voided == false).Sum(t => t.Amount);
                    }
                    else if (Frequency != null && Frequency.Name == "Yearly")
                    {
                        return Category.Transactions.Where(t => t.TransactionDate.Year == DateTime.Now.Year && t.Voided == false).Sum(t => t.Amount);
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

        public virtual Frequency Frequency { get; set; }
        public virtual Household Household { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser Author { get; set; }

        
    }
}