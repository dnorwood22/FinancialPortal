using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using FinancialPortal.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using FinancialPortal.Models.Helpers;

namespace FinancialPortal.Controllers
{
    [AuthorizeHouseholdRequired]
    public class AccountTransactionsController : Universal
    {

        // GET: AccountTransactions
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var accountTransactions = user.Household.Accounts.SelectMany(a => a.Transactions).ToList();
            return View(accountTransactions.ToList());
        }

        // GET: AccountTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            if (accountTransaction == null)
            {
                return HttpNotFound();
            }
            return View(accountTransaction);
        }

        // GET: AccountTransactions/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.AccountId = new SelectList(user.Household.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type");
            return View();
        }

        // POST: AccountTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,CategoryId,AccountId,AccountTypeId,TransactionTypeId,Amount,TransactionDate")] AccountTransaction accountTransaction)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                
                var household = user.Household;
                var updated = false;
                accountTransaction.AuthorId = user.Id;
                accountTransaction.Created = DateTime.Now;
                accountTransaction.Voided = false;
                db.Transactions.Add(accountTransaction);

                Account account = db.Accounts.Find(accountTransaction.AccountId);
                
                
                if(accountTransaction.TransactionTypeId == 1)
                {
                    accountTransaction.Amount *= -1; 
                    account.Balance += accountTransaction.Amount;
                    updated = true;
                }
                else if (accountTransaction.TransactionTypeId == 2)
                {
                    account.Balance += accountTransaction.Amount;
                    updated = true;
                }
                db.SaveChanges();
                if (updated == true && account.Household != null)
                {
                    if (account.Balance == 0)
                    {
                        foreach (var u in household.Users)
                        {
                            Notification n = new Notification();
                            n.NotifyUserId = account.HouseholdId.ToString();
                            n.Created = DateTime.Now;
                            n.AccountId = account.Id;
                            n.Type = "Zero Dollars";
                            n.Description = "Your account: " + account.Name + " has reached an amount of zero.";
                            db.Notifications.Add(n);
                            db.SaveChanges();
                        }
                    }
                    
                   
                   else if (account.Balance < 0)
                    {
                        foreach (var u in household.Users )
                        {
                            Notification n = new Notification();
                            n.NotifyUserId = u.Id;
                            n.Created = DateTime.Now;
                            n.AccountId = account.Id;
                            n.Type = "Over Draft";
                            n.Description = "Your account: " + account.Name + " has reached a negative amount.";
                            db.Notifications.Add(n);
                            db.SaveChanges();
                        }
                    }

                    account.Updated = DateTime.Now;
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(user.Household.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", accountTransaction.CategoryId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type", accountTransaction.TransactionTypeId);
            return View(accountTransaction);
        }

        // GET: AccountTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            if (accountTransaction == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", accountTransaction.CategoryId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type", accountTransaction.TransactionTypeId);
            return View(accountTransaction);
        }

        // POST: AccountTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AuthorId,CategoryId,AccountId,AccountTypeId,TransactionTypeId,Amount,ReconciledStatus,Voided,ReconciledAmount,Created,TransactionDate")] AccountTransaction accountTransaction)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                accountTransaction.AuthorId = user.Id;
                accountTransaction.Created = DateTime.Now;
                db.Entry(accountTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", accountTransaction.CategoryId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type", accountTransaction.TransactionTypeId);
            return View(accountTransaction);
        }

        // GET: AccountTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            if (accountTransaction == null)
            {
                return HttpNotFound();
            }
            return View(accountTransaction);
        }

        // POST: AccountTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            db.Transactions.Remove(accountTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AccountTransactions/Voided
        public ActionResult Voided(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            if (accountTransaction == null)
            {
                return HttpNotFound();
            }
            if (accountTransaction.Voided == true)
            {
                return RedirectToAction("UnVoid", new { id = id });
            }

            return View(accountTransaction);
        }

        // POST: AccountTransactions/Voided
        [HttpPost, ActionName("Voided")]
        [ValidateAntiForgeryToken]
        public ActionResult VoidConfirmed(int id)
        {
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            Account account = db.Accounts.Find(accountTransaction.AccountId);
            account.Balance -= accountTransaction.Amount;
            accountTransaction.Voided = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AccountTransactions/UnVoid
        public ActionResult UnVoid(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            if (accountTransaction == null)
            {
                return HttpNotFound();
            }
            if (accountTransaction.Voided == false)
            {
                return RedirectToAction("Voided");
            }

            return View(accountTransaction);
        }

        // POST: AccountTransactions/Voided
        [HttpPost, ActionName("UnVoid")]
        [ValidateAntiForgeryToken]
        public ActionResult UnVoidConfirmed(int id)
        {
            AccountTransaction accountTransaction = db.Transactions.Find(id);
            Account account = db.Accounts.Find(accountTransaction.AccountId);
            account.Balance += accountTransaction.Amount;
            accountTransaction.Voided = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
