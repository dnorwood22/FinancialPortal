namespace FinancialPortal.Migrations
{
    using FinancialPortal.Models;
    using FinancialPortal.Models.CodeFirst;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinancialPortal.Models.ApplicationDbContext context)
        {

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "dnorwood22@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dnorwood22@gmail.com",
                    Email = "dnorwood22@gmail.com",
                    TimeZone = "US Eastern Standard Time",
                    FirstName = "Dexter",
                    LastName = "Norwood",
                }, "Jord@n23");

            }


            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Car";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Childcare";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Clothes";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Fees";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Education";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Events";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Charity";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Food";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Gifts";
                context.Categories.Add(category);
            }

            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Mortgage";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Insurance";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Travel";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Utilities";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Rent";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Expense"))
            {
                var category = new Category();
                category.Name = "Other";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Pay Check";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Bonus";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Investments";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Interest";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Winnings";
                context.Categories.Add(category);
            }
            if (!context.Categories.Any(p => p.Name == "Income"))
            {
                var category = new Category();
                category.Name = "Other";
                context.Categories.Add(category);
            }
            if (!context.AccountTypes.Any(p => p.Name == "Checking"))
            {
                var accounttype = new AccountType();
                accounttype.Name = "Checking";
                context.AccountTypes.Add(accounttype);
            }
            if (!context.AccountTypes.Any(p => p.Name == "Savings"))
            {
                var accounttype = new AccountType();
                accounttype.Name = "Savings";
                context.AccountTypes.Add(accounttype);
            }
            if (!context.AccountTypes.Any(p => p.Name == "Credit"))
            {
                var accounttype = new AccountType();
                accounttype.Name = "Credit";
                context.AccountTypes.Add(accounttype);
            }
        }
    }
}
