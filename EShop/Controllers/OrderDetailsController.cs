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
            bool flag = false;
            var List = new List<Cart>();
            Cart c = new Cart(id,quantity,Price,db.Product.Find(id).Pro_Image.Split('$').First(),db.Product.Find(id).Pro_Name);
            List.Add(c);
            if (Session["ListCart"] != null)
            {
                var exiting = Session["ListCart"] as List<Cart>;
                if (exiting.Exists(e => e.Pro_id == id))
                {
                    exiting.Where(ex => ex.Pro_id == id).ToList().ForEach(i => i.Quantity += quantity);
                }
                else
                {
                    exiting.Add(c);
                }
                Session["ListCart"] = exiting;
               
            }
            else {
                Session["ListCart"] = List;
              
            }
           
            return Redirect("~/Products/Details/"+id);
        }
        public ActionResult AddQuantity(int id) {
            var list = Session["ListCart"] as List<Cart>;
            list.Where(ex => ex.Pro_id == id).ToList().ForEach(i => i.Quantity += 1);
            Session["ListCart"] = list;
            return RedirectToAction("Create");
        }
        public ActionResult MinusQuantity(int id)
        {
            var list = Session["ListCart"] as List<Cart>;
            list.Where(ex => ex.Pro_id == id).ToList().ForEach(i => i.Quantity -= 1);
            if (list.Where(ex => ex.Pro_id == id).FirstOrDefault().Quantity <= 0) {
                list.RemoveAll(l => l.Pro_id == id);
            }
            Session["ListCart"] = list;
            return RedirectToAction("Create");
        }
        public ActionResult Create() {
            Customers c = Session["Customer"] as Customers;
            if (c != null) {
                ViewBag.Name = c.Cus_Name;
                ViewBag.Adress = c.Cus_Adress;
                ViewBag.Phone = c.Cus_PhoneNumber;
                return View();
            }
            return View();
        }
        [HttpPost]    
        public ActionResult Create(string phone,string adress,string name)
        {           
            Customers c = Session["Customer"] as Customers;
            var list=Session["ListCart"] as List<Cart>;
            if (list != null) {
                Orders o = new Orders();
                o.Ord_CreateDate = DateTime.Now.ToString();
                o.Ord_ToAdress = adress;
                o.Ord_ToPhone = phone;
                o.Ord_ToName = name;
                o.Ord_Status = 0;
                if (c != null)
                {
                    o.Ord_Customer = c.Cus_Id;                                  
                }
                db.Orders.Add(o);
                db.SaveChanges();
                foreach (var item in list) {
                    OrderDetails od = new OrderDetails();
                    od.Detail_OrderNumber = o.Ord_Id;
                    od.Detail_Product = item.Pro_id;
                    od.Detail_Quantity = item.Quantity;
                    od.Detail_PriceEach = item.Price;
                    db.OrderDetails.Add(od);
                    db.SaveChanges();
                }
                Session["ListCart"] = null;
                return Redirect("~/Home/Index");
            }
            return View();
        }
        public ActionResult RemoveItem(int id) {
            var list = Session["ListCart"] as List<Cart>;
            list.RemoveAll(c=>c.Pro_id==id);             
            Session["ListCart"] = list;
            return RedirectToAction("Create");
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
