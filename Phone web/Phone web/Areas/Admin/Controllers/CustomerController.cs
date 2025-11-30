using Phone_web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Phone_web.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        WebEntities db = new WebEntities();

        // Danh sách khách hàng
        public ActionResult khachhang()
        {
            return View(db.Customers.ToList());
        }

        // Thêm KH - GET
        [HttpGet]
        public ActionResult themkh()
        {
            return View();
        }

        // Thêm KH - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult themkh(Customer kh)
        {
            if (ModelState.IsValid)
            {
                kh.CreateDate = DateTime.Now;
                kh.LastLogin = DateTime.Now;

                db.Customers.Add(kh);
                db.SaveChanges();

                return RedirectToAction("khachhang");
            }

            return View(kh);
        }

        // Sửa KH - GET
        [HttpGet]
        public ActionResult suakh(string id)
        {
            var kh = db.Customers.Find(id);
            if (kh == null) return HttpNotFound();

            return View(kh);
        }

        // Sửa KH - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult suakh(Customer kh)
        {
            if (!ModelState.IsValid)
                return View(kh);

            // Tìm khách hàng theo khóa chính (CusPhone)
            var existing = db.Customers.Find(kh.CusPhone);
            if (existing == null) return HttpNotFound();

            // KHÔNG ĐƯỢC ĐỔI CusPhone vì là khóa chính
            // existing.CusPhone = existing.CusPhone;

            existing.CusName = kh.CusName;
            existing.CusEmail = kh.CusEmail;
            existing.Role = kh.Role;
            existing.CusPassword = kh.CusPassword;
            existing.CreateBy = kh.CreateBy;

            db.SaveChanges();

            return RedirectToAction("khachhang");
        }

        // Xóa KH - GET
        [HttpGet]
        public ActionResult xoakh(string id)
        {
            var kh = db.Customers.Find(id);
            if (kh == null) return HttpNotFound();

            return View(kh);
        }

        // Xóa KH - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xoakh(string id,Customer kh)
        {
             kh = db.Customers.Find(id);
            if (kh != null)
            {
                db.Customers.Remove(kh);
                db.SaveChanges();
            }

            return RedirectToAction("khachhang");
        }

        // Chi tiết KH
        public ActionResult chitietkh(string id)
        {
            var kh = db.Customers.Find(id);
            if (kh == null) return HttpNotFound();

            return View(kh);
        }
    }
}
