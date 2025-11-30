using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        WebEntities db = new WebEntities();

        // GET: Admin/Category
        public ActionResult danhmuc()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        // GET: form thêm danh mục
        [HttpGet]
        public ActionResult Them()
        {
            return View();
        }

        // POST: thêm danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Them(Category cate)
        {
            if (!ModelState.IsValid)
            {
                return View(cate); // kiểm tra lỗi validation khác
            }

            var name = cate.CatName.Trim().ToLower();
            var exists = db.Categories.Any(c => c.CatName.Trim().ToLower() == name);

            if (exists)
            {
                ModelState.AddModelError("CatName", "Danh mục đã tồn tại, vui lòng nhập tên khác.");
                return View(cate);
            }

            cate.CatName = cate.CatName.Trim(); // lưu tên đã trim
            db.Categories.Add(cate);
            db.SaveChanges();

            return RedirectToAction("danhmuc");
        }

        // GET: form sửa danh mục
        [HttpGet]
        public ActionResult Sua(int id)
        {
            var cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();

            return View(cate);
        }

        // POST: sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(int id, Category cate)
        {
            if (!ModelState.IsValid)
            {
                return View(cate);
            }

            var cate2 = db.Categories.Find(id);
            if (cate2 == null)
                return HttpNotFound();

            var name = cate.CatName.Trim().ToLower();
            var exists = db.Categories.Any(c => c.CatName.Trim().ToLower() == name && c.CatID != id);

            if (exists)
            {
                ModelState.AddModelError("CatName", "Tên danh mục đã tồn tại, vui lòng nhập tên khác.");
                return View(cate);
            }

            cate2.CatName = cate.CatName.Trim();
            db.SaveChanges();

            return RedirectToAction("danhmuc");
        }

        // GET: xóa danh mục
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            var cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();

            var hasProducts = db.Products.Any(p => p.CatID == id);
            if (hasProducts)
            {
                ViewBag.Error = "Không thể xóa vì danh mục này đang chứa sản phẩm!";
            }

            return View(cate);
        }

        // POST: xóa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoa(int id, Category cate)
        {
            cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();

            var sp = db.Products.Where(p => p.CatID == id).ToList();
            db.Products.RemoveRange(sp);

            db.Categories.Remove(cate);
            db.SaveChanges();

            return RedirectToAction("danhmuc");
        }

        // GET: chi tiết danh mục
        public ActionResult Chitiet(int id)
        {
            var cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();

            return View(cate);
        }
    }
}
