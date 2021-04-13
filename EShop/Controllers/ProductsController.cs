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
using System.IO;
using EShop.Repository;

namespace EShop.Controllers
{
    public class ProductsController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();
        Repo r = new Repo();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var product = db.Product.Include(p => p.Categories);
            return View(await product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/Index");
            }
            Product product = await db.Product.Include(b=>b.Brands).Where(p=>p.Pro_Id==id).FirstOrDefaultAsync();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Pro_Brand = new SelectList(db.Brands, "Brand_Id", "Brand_Name");
            ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Pro_Id,Pro_Name,Pro_Price,Pro_Description,Pro_Brand,Pro_Category")] Product product, HttpPostedFileBase[] Url)
        {

            int flag = 0;
           
            string url_img = "";
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (HttpPostedFileBase img in Url)
                    {
                        if (img != null)
                        {

                            string ex = Path.GetExtension(img.FileName);
                            if (!r.check(ex, formats))
                            {
                                flag = 1;
                                ViewBag.FileStatus = ex + " is not an image";
                                ViewBag.Pro_Brand = new SelectList(db.Brands, "Brand_Id", "Brand_Name");
                                ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name");
                                return View(product);
                            }
                            url_img += Path.GetFileName(img.FileName) + "$";
                            product.Pro_Image = url_img.Substring(0, url_img.Length - 1);
                        }
                        else
                        {
                            flag = 1;
                            ViewBag.FileStatus = " Must have image !!!!";
                            ViewBag.Pro_Brand = new SelectList(db.Brands, "Brand_Id", "Brand_Name");
                            ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name");
                            return View(product);
                        }
                        if (flag != 1)
                        {
                            string path = Path.Combine(Server.MapPath("~/images/Products"), Path.GetFileName(img.FileName));
                            img.SaveAs(path);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
                db.Product.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Pro_Brand = new SelectList(db.Brands, "Brand_Id", "Brand_Name");
            ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name", product.Pro_Category);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name", product.Pro_Category);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Pro_Id,Pro_Name,Pro_Price,Pro_Image,Pro_Description,Pro_Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Pro_Category = new SelectList(db.Categories, "Cate_Id", "Cate_Name", product.Pro_Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Product.FindAsync(id);
            db.Product.Remove(product);
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
