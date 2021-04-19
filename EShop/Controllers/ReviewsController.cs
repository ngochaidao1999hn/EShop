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

namespace EShop.Controllers
{
    public class ReviewsController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();

        // GET: Reviews
        public async Task<ActionResult> Index()
        {
            var reviews = db.Reviews.Include(r => r.Customers).Include(r => r.Product);
            return View(await reviews.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.Review_CustomerId = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            ViewBag.Review_ProductId = new SelectList(db.Product, "Pro_Id", "Pro_Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public  ActionResult Create(int Cus_id,int Pro_id,string Content,int star)
        {
            Reviews review = new Reviews();
            review.Review_ProductId = Pro_id;
            review.Review_CustomerId = Cus_id;
            review.Review_Content = Content;
            review.Review_Rate = star;
            review.Review_DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                db.Reviews.Add(review);
               db.SaveChanges();
                return Redirect("/Products/Details/"+Pro_id);
            

          
            
        }

        // GET: Reviews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.Review_CustomerId = new SelectList(db.Customers, "Cus_Id", "Cus_Name", reviews.Review_CustomerId);
            ViewBag.Review_ProductId = new SelectList(db.Product, "Pro_Id", "Pro_Name", reviews.Review_ProductId);
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Review_Id,Review_CustomerId,Review_ProductId,Review_DateTime,Review_Content,Review_Rate")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviews).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Review_CustomerId = new SelectList(db.Customers, "Cus_Id", "Cus_Name", reviews.Review_CustomerId);
            ViewBag.Review_ProductId = new SelectList(db.Product, "Pro_Id", "Pro_Name", reviews.Review_ProductId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Reviews reviews = await db.Reviews.FindAsync(id);
            db.Reviews.Remove(reviews);
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
