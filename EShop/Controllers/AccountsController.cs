using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EShop.Models;
using EShop.Repository;

namespace EShop.Controllers
{
    public class AccountsController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();
        Repo r = new Repo();

        // GET: Accounts
        public async Task<ActionResult> Index()
        {
            var accounts = db.Accounts.Include(a => a.Customers);
            return View(await accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName,string Password) {
            string password = r.HashPwd(Password);
            var acc = db.Accounts.Where(acc => acc.Acc_UserName == UserName && acc.Acc_Password == password).FirstOrDefault();
            if (acc == null) {
                ViewBag.Error = "Username or password not correct!";
                return View();
            }
            Session["Customer"] = db.Customers.Find(acc.Customers.Cus_Id);
            return Redirect("~/Home/Index");        
        }
        public ActionResult Logout() {
            if (Session["Customer"] != null)
            {
                Session["Customer"] = null;
               
            }
            return Redirect("~/Home/Index");
        }
        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.Acc_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public  ActionResult Create(string Cus_Name,string Cus_Adress, string Cus_Phone,string Cus_Email,string Cus_DOB,string Acc_Pass,string Acc_UserName,string Acc_RePass)
        {
           
                Customers cus = new Customers();
                cus.Cus_Name = Cus_Name;
                cus.Cus_Email = Cus_Email;
                cus.Cus_Adress = Cus_Adress;
                cus.Cus_Dob = Cus_DOB;
                cus.Cus_PhoneNumber = Cus_Phone;
            if (Acc_Pass == Acc_RePass)
            {
                string password = r.HashPwd(Acc_Pass); 
                if (db.Accounts.Where(acc => acc.Acc_UserName == Acc_UserName && acc.Acc_Password == password).FirstOrDefault() == null)
                {
                    db.Customers.Add(cus);
                    db.SaveChanges();
                    Accounts acc = new Accounts();
                    acc.Acc_UserName = Acc_UserName;
                    acc.Acc_Password = r.HashPwd(Acc_Pass);
                    acc.Acc_Customer = cus.Cus_Id;
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    Session["Customer"] = cus;
                    return Redirect("~/Home/Index");

                }
                ViewBag.ErrorRegister = "Account already exxist";
                ViewBag.Cus_Name = Cus_Name;
                ViewBag.Cus_Adress = Cus_Adress;
                ViewBag.Cus_Email = Cus_Email;
                ViewBag.Cus_Phone = Cus_Phone;
                ViewBag.Cus_Dob = Cus_DOB;
                ViewBag.UserName = Acc_UserName;
                ViewBag.Pass = Acc_Pass;
                return View();
            }
            ViewBag.ErrorRegister = "Repassword not correct";
            ViewBag.Cus_Name = Cus_Name;
            ViewBag.Cus_Adress = Cus_Adress;
            ViewBag.Cus_Email = Cus_Email;
            ViewBag.Cus_Phone = Cus_Phone;
            ViewBag.Cus_Dob = Cus_DOB;
            ViewBag.UserName = Acc_UserName;
            ViewBag.Pass = Acc_Pass;
            return View();
        }

        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.Acc_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name", accounts.Acc_Customer);
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Acc_Id,Acc_UserName,Acc_Password,Acc_Customer")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accounts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Acc_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name", accounts.Acc_Customer);
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Accounts accounts = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(accounts);
            await db.SaveChangesAsync();
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
