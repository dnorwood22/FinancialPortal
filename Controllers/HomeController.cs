using FinancialPortal.Models;
using FinancialPortal.Models.CodeFirst;
using FinancialPortal.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HomeController : Universal
    {
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.TotalIncome = user.Household.Accounts.SelectMany(a => a.Transactions.Where(t => t.Voided == false && t.Amount > 0)).Sum(t => t.Amount);
            ViewBag.TotalExpense = Math.Abs(user.Household.Accounts.SelectMany(a => a.Transactions.Where(t => t.Voided == false && t.Amount < 0)).Sum(t => t.Amount));
            return View(user.Household);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }


        public async Task<ActionResult> LeaveHousehold()
        {
            //Implementation of leaving household

            var user = db.Users.Find(User.Identity.GetUserId());
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            return View();

            //await HttpContext.RefreshAuthentication(db.Users.Find(User.Identity.GetUserId()));
        }

        //POST: CreateJoinHousehold/Home
        public async Task<ActionResult> CreateJoinHousehold([Bind(Include = "Id,Name,AuthorId")] Household household)
        {
            //Implementation for creating and joining household
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());


                db.Households.Add(household);
                db.SaveChanges();
                user.HouseholdId = household.Id;
                db.SaveChanges();


                await HttpContext.RefreshAuthentication(db.Users.Find(User.Identity.GetUserId()));
                return RedirectToAction("Index", "Households");
            }

            return View();

        }
        [AllowAnonymous]
        public ActionResult LandingPage()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}