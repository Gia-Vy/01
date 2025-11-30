using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Phone_web.Areas.Admin.Controllers
{
    public class ProductDetailController : Controller
    {
        WebEntities db = new WebEntities();
        // GET: Adminn/ProductDetail

        public ActionResult khohang()
        {
            var list = db.ProductDetails.ToList();
            return View(list);
        }

            [HttpGet]
        public ActionResult Themsp()
        {
            ViewBag.Products = db.Products.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.DungLuongs = db.DungLuongs.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themsp(ProductDetail sp)
        {
            ViewBag.Products = db.Products.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.DungLuongs = db.DungLuongs.ToList();

            // Kiểm tra số lượng
            if (sp.RemainQuantity < 0)
                ModelState.AddModelError("RemainQuantity", "Số lượng tồn không được âm.");
            if (sp.SoldQuantity < 0)
                ModelState.AddModelError("SoldQuantity", "Số lượng bán không được âm.");

            // Kiểm tra trùng ProID + ColorID + DungLuongID
            bool exists = db.ProductDetails.Any(p => p.ProID == sp.ProID
                                                   && p.ColorID == sp.ColorID
                                                   && p.DungLuongID == sp.DungLuongID);

            if (exists)
                ModelState.AddModelError("", "Chi tiết sản phẩm này đã tồn tại.");

            if (!ModelState.IsValid)
                return View(sp);

            db.ProductDetails.Add(sp);
            db.SaveChanges();
            return RedirectToAction("khohang");
        }
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            var detail = db.ProductDetails.Find(id);
            if (detail == null)
                return HttpNotFound();

            ViewBag.Products = db.Products.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.DungLuongs = db.DungLuongs.ToList();
            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suasp(ProductDetail sp)
        {
            ViewBag.Products = db.Products.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.DungLuongs = db.DungLuongs.ToList();

            // Kiểm tra số lượng không âm
            if (sp.RemainQuantity < 0)
                ModelState.AddModelError("RemainQuantity", "Số lượng tồn không được âm.");
            if (sp.SoldQuantity < 0)
                ModelState.AddModelError("SoldQuantity", "Số lượng bán không được âm.");

            // Kiểm tra trùng combo, bỏ chính nó
            bool exists = db.ProductDetails.Any(p => p.ProID == sp.ProID
                                                   && p.ColorID == sp.ColorID
                                                   && p.DungLuongID == sp.DungLuongID
                                                   && p.ProDeID != sp.ProDeID);
            if (exists)
                ModelState.AddModelError("", "Chi tiết sản phẩm này đã tồn tại.");

            if (!ModelState.IsValid)
                return View(sp);

            var detail = db.ProductDetails.Find(sp.ProDeID);
            if (detail == null) return HttpNotFound();

            detail.ProID = sp.ProID;
            detail.ColorID = sp.ColorID;
            detail.DungLuongID = sp.DungLuongID;
            detail.Price = sp.Price;
            detail.RemainQuantity = sp.RemainQuantity;
            detail.SoldQuantity = sp.SoldQuantity;

            db.SaveChanges();
            return RedirectToAction("khohang");
        }




        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            var detail = db.ProductDetails.Find(id);
            if (detail == null) return HttpNotFound();
            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoasp(int id, ProductDetail detail)
        {
            detail = db.ProductDetails.Find(id);
            if (detail == null) return HttpNotFound();

            db.ProductDetails.Remove(detail);
            db.SaveChanges();
            return RedirectToAction("khohang");
        }


        public ActionResult Chitietsp(int id)
        {
            var sp = db.ProductDetails.Find(id);
            if (sp == null) return HttpNotFound();
            return View(sp);
        }
    }
}