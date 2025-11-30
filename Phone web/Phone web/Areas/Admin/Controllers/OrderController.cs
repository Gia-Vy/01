using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        WebEntities db = new WebEntities();

        // GET: Admin/Order
        public ActionResult donhang()
        {
            var orders = db.Orders.ToList();
            return View(orders);
        }
       



        [HttpGet]
        public ActionResult themdh()
        {
            ViewBag.Customers = db.Customers.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult themdh(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = db.Customers.ToList();
                return View(order);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("donhang");
        }




        [HttpGet]
        public ActionResult suadh(int id)
        {
             var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            ViewBag.Customers = db.Customers.ToList();
            return View(order);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult suadh(Order or)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = db.Customers.ToList();
                return View(or);
            }

            var order = db.Orders.Find(or.OrderID);

            order.CusPhone = or.CusPhone;
            order.AddressDelivery = or.AddressDelivery;
            order.TotalValue = or.TotalValue;
            order.PaymentMethod = or.PaymentMethod;
            order.PaymentStatus = or.PaymentStatus;
            order.OrderStatus = or.OrderStatus;
            order.OrderDate = or.OrderDate;

            db.SaveChanges();
            return RedirectToAction("donhang");
        }


        //public ActionResult chittietdh(int id)
        //{
        //    var order = db.Orders.Find(id);
        //    if (order == null) return HttpNotFound();

        //    return View(order);
        //}




        [HttpGet]
        public ActionResult xoadh(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xoadh(int id,Order order)
        {
            order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("donhang");
        }
        public ActionResult chittietdh(int? id)
        {
            if (!id.HasValue) return RedirectToAction("donhang"); // hoặc hiển thị lỗi
            var order = db.Orders.Find(id.Value);
            if (order == null) return HttpNotFound();
            return View(order);
        }


    }

}