using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Areas.Admin.Controllers
{
    
    public class OrderDetailController : Controller
    {
      
        WebEntities db = new WebEntities();
        // GET: Admin/OrderDetail
        public ActionResult chitietdonhang()
        {
            var orderDetails = db.OrderDetails.ToList();
            return View(orderDetails);
        }

        [HttpGet]
        public ActionResult them()
        {
            ViewBag.Orders = db.Orders.ToList();
            ViewBag.ProductDetails = db.ProductDetails.ToList();

          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult them(OrderDetail ordetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(ordetail);
                db.SaveChanges();
                return RedirectToAction("chitietdonhang");
            }

            ViewBag.Orders = db.Orders.ToList();
            ViewBag.ProductDetails = db.ProductDetails.ToList();
            return View(ordetail);
        }

        [HttpGet]
        public ActionResult sua(int id)
        {
            var ordetail = db.OrderDetails.Find(id);
            if (ordetail == null) return HttpNotFound();

            ViewBag.Orders = db.Orders.ToList();
            ViewBag.ProductDetails = db.ProductDetails.ToList();
            return View(ordetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult sua(OrderDetail ordetail)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = db.Orders.ToList();
                ViewBag.ProductDetails = db.ProductDetails.ToList();
                return View(ordetail);
            }

            var detail = db.OrderDetails.Find(ordetail.OrderDeID);
            if (detail == null) return HttpNotFound();

            detail.OrderID = ordetail.OrderID;
            detail.ProDeID = ordetail.ProDeID;
            detail.Quantity = ordetail.Quantity;
            detail.UnitPrice = ordetail.UnitPrice;

            db.SaveChanges();
            return RedirectToAction("chitietdonhang");
        }


        [HttpGet]
        public ActionResult xoa(int id, OrderDetail ordetail)
        {
            ordetail = db.OrderDetails.Find(id);
            if (ordetail == null) return HttpNotFound();
            return View(ordetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xoa(int id)
        {
            var ordetail = db.OrderDetails.Find(id);
            if (ordetail == null) return HttpNotFound();

            db.OrderDetails.Remove(ordetail);
            db.SaveChanges();

            return RedirectToAction("chitietdonhang");
        }

        public ActionResult chitiet(int id)
        {
            var ordetail = db.OrderDetails.Find(id);
            if (ordetail == null) return HttpNotFound();
            return View(ordetail);
        }

    }
}