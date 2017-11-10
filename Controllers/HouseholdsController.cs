using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using FinancialPortal.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using FinancialPortal.Models.Helpers;
using System.Net.Mail;
using System.Net;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HouseholdsController : Universal
    {
        // GET: Households
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
           var user = db.Users.Find(User.Identity.GetUserId());
         
            return View(user.Household);


        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {

             return View();  
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title")] Household household)
        {
           
            

           if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                household.AuthorId = user.Id;
                db.Households.Add(household);
                db.SaveChanges();

                user.HouseholdId = household.Id;
                db.SaveChanges();


                await HttpContext.RefreshAuthentication(user);
                return RedirectToAction("Index");

            }
                return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Join
        public ActionResult Join(int id)
        {
            
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        //Post: Households/Join
        [HttpPost]
        public async Task<ActionResult> Join(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = household.Id;
            db.SaveChanges();

            await HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Index");

        }

        // GET: HouseHolds/Invite
        public ActionResult Invite()
        {
            return View();
        }

        //Post: HouseHolds/Invite
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var invited = db.Users.FirstOrDefault(u => u.Email == model.EmailTo);
                    var me = db.Users.Find(User.Identity.GetUserId());
                   
                    if (invited != null && invited.HouseholdId == model.HouseholdId)
                    {
                        return RedirectToAction("Households");
                    }
                    var callbackUrl = "";
                    if (invited != null)
                    {
                        callbackUrl = Url.Action("Join", "Households", new { id = model.HouseholdId }, protocol: Request.Url.Scheme);
                    }
                    else
                    {
                        callbackUrl = Url.Action("Register", "Account", new { id = model.HouseholdId }, protocol: Request.Url.Scheme);
                    }
                    var body = "<p>Email From: <bold>{0}</bold></p><p>Message:</p><p>{1}</p><p>{2}</p>";
                    var from = "Financial Portal<" + me.Email + ">";
                    var subject = "Invite to join HouseHold";
                    var to = model.EmailTo;

                    var email = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = string.Format(body, me.FullName, model.Body, "Please click on the link below to confirm invitation: <br /> <a href=\" " + callbackUrl + " \">Link to invitation.</a>"),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return RedirectToAction("InviteSent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

        //GET: Households/InviteSent
        public ActionResult InviteSent()
        {
            return View();
        }

        // GET: Households/Leave
        public ActionResult Leave()
        {
            var currentHouseholdId = User.Identity.GetHouseholdId();
            var currentHousehold = db.Households.Find(currentHouseholdId);
            return View(currentHousehold);
        }

        //POST: Household/Leave 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Leave(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = null;
            db.SaveChanges();

            await HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Index","Home");
        }

        // GET: Households/Voided
        public ActionResult Voided(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Voided
        [HttpPost, ActionName("Voided")]
        [ValidateAntiForgeryToken]
        public ActionResult VoidedConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
