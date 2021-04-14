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
    public class OrdersController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.Customers);
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = await db.Orders.FindAsync(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Ord_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Ord_Id,Ord_CreateDate,Ord_ShippedDate,Ord_Status,Ord_Customer")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Ord_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name", orders.Ord_Customer);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = await db.Orders.FindAsync(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ord_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name", orders.Ord_Customer);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Ord_Id,Ord_CreateDate,Ord_ShippedDate,Ord_Status,Ord_Customer")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ord_Customer = new SelectList(db.Customers, "Cus_Id", "Cus_Name", orders.Ord_Customer);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = await db.Orders.FindAsync(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Orders orders = await db.Orders.FindAsync(id);
            db.Orders.Remove(orders);
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
