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
using EShop.Data;

namespace EShop.Controllers
{
    public class OrderDetailsController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();
        HttpCookie CartInfo = new HttpCookie("CartInfo");
        // GET: OrderDetails
        public async Task<ActionResult> Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Orders).Include(o => o.Product);
            return View(await orderDetails.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }
        public ActionResult AddToCart(int id, int quantity, decimal Price) {
            var List = new List<Cart>();
            Cart c = new Cart(id,quantity,Price);
            List.Add(c);
            if (Session["ListCart"] != null)
            {
                var exiting = Session["ListCart"] as List<Cart>;
                exiting.AddRange(List);
                Session["ListCart"] = exiting;
               
            }
            else {
                Session["ListCart"] = List;
              
            }
           
            return Redirect("~/Products/Details/"+id);
        }
        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.Detail_OrderNumber = new SelectList(db.Orders, "Ord_Id", "Ord_CreateDate");
            ViewBag.Detail_Product = new SelectList(db.Product, "Pro_Id", "Pro_Name");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Detail_Product,Detail_Quantity,Detail_PriceEach,Detail_OrderNumber")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Detail_OrderNumber = new SelectList(db.Orders, "Ord_Id", "Ord_CreateDate", orderDetails.Detail_OrderNumber);
            ViewBag.Detail_Product = new SelectList(db.Product, "Pro_Id", "Pro_Name", orderDetails.Detail_Product);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.Detail_OrderNumber = new SelectList(db.Orders, "Ord_Id", "Ord_CreateDate", orderDetails.Detail_OrderNumber);
            ViewBag.Detail_Product = new SelectList(db.Product, "Pro_Id", "Pro_Name", orderDetails.Detail_Product);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Detail_Product,Detail_Quantity,Detail_PriceEach,Detail_OrderNumber")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Detail_OrderNumber = new SelectList(db.Orders, "Ord_Id", "Ord_CreateDate", orderDetails.Detail_OrderNumber);
            ViewBag.Detail_Product = new SelectList(db.Product, "Pro_Id", "Pro_Name", orderDetails.Detail_Product);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            db.OrderDetails.Remove(orderDetails);
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
