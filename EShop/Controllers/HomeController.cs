using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class HomeController : Controller
    {
        private EShopDbEntities db = new EShopDbEntities();
        public ActionResult Index()
        {
            ViewBag.ListProduct = db.Product.OrderByDescending(p => p.Pro_Id).Take(6).ToList();
                                  
            return View();
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
    }
}