﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FinancialPortal.Models.CodeFirst;
using System.Collections.Generic;

namespace FinancialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimeZone { get; set; }
        public int? HouseholdId { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ApplicationUser()
        {
            Transactions = new HashSet<AccountTransaction>();
            Notifications = new HashSet<Notification>();
        }

        public virtual Household Household { get; set; }
        public virtual ICollection<AccountTransaction> Transactions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("HouseholdId", HouseholdId.ToString()));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<AccountTransaction> Transactions { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Budget>Budgets { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        
             
    }
}