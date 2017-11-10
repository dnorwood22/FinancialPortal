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

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class AccountTransactionsController : Universal
    {

        // GET: AccountTransactions
        public ActionResult Index()
        {
            var accountTransactions = db.Transactions.Include(a => a.Account).Include(a => a.AccountType).Include(a => a.Author).Include(a => a.Category).Include(a => a.TransactionType);
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
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type");
            return View();
        }

        // POST: AccountTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,CategoryId,AccountId,AccountTypeId,TransactionTypeId,Amount,TransactionDate")] AccountTransaction accountTransaction)
        {
            

            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var updated = false;
                accountTransaction.AuthorId = user.Id;
                accountTransaction.Created = DateTime.Now;
                accountTransaction.Voided = false;
                db.Transactions.Add(accountTransaction);

                Account account = db.Accounts.Find(accountTransaction.AccountId);
                
                
                if(accountTransaction.TransactionTypeId == 1)
                {
                    account.Balance -= accountTransaction.Amount;
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
                    Notification n = new Notification();
                    n.NotifyUser = account.Household;
                    n.Created = DateTime.Now;
                    n.Type = "Zero Dollars";
                    n.Description = "Your account has reached an amount of zero.";
                    db.Notifications.Add(n);
                    db.SaveChanges();
                    }
                    
                   
               if (account.Balance < 0)
                    {
                    Notification n = new Notification();
                    n.NotifyUser = account.Household.Value();
                    n.Created = DateTime.Now;
                    n.Type = "Over Draft";
                    n.Description = "Your account has reached a negative amount.";
                    db.Notifications.Add(n);
                    db.SaveChanges();
                    }

                    account.Updated = DateTime.Now;
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "Id", "Name", accountTransaction.AccountTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", accountTransaction.AuthorId);
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
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "Id", "Name", accountTransaction.AccountTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", accountTransaction.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", accountTransaction.CategoryId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Type", accountTransaction.TransactionTypeId);
            return View(accountTransaction);
        }

        // POST: AccountTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AuthorId,CategoryId,AccountId,AccountTypeId,TransactionTypeId,Amount,ReconciledStatus,Voided,ReconciledAmount,Created,TransactionDate,ReconciliationDate")] AccountTransaction accountTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", accountTransaction.AccountId);
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "Id", "Name", accountTransaction.AccountTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", accountTransaction.AuthorId);
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
