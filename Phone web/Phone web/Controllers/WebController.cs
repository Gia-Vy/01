using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Controllers
{
    public class WebController : Controller
    {
        WebEntities db = new WebEntities();
        // GET: Admin/Category
        public ActionResult danhmuc()
        {
            var list = db.Categories.ToList(); // trả về IEnumerable<Category>
            return View(list);
        }
        public ActionResult sanpham(int? categoryId)
        {
            var list = db.Products.ToList();
            return View(list);
        }
    }
}