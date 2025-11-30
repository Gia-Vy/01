using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Phone_web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebEntities db = new WebEntities();


        // GET: Admin/Product
        //lồng chức năng của thanh tìm kiếm
        public ActionResult sanpham(int? categoryId, string search)
        {
            var list = db.Products.AsQueryable();

            // Lọc theo category nếu có
            if (categoryId.HasValue)
            {
                list = list.Where(p => p.CatID == categoryId.Value);
            }

            // Lọc theo từ khóa search
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(p => p.ProName.Contains(search));
            }

            // Eager load category
            list = list.OrderBy(p => p.ProName).AsQueryable();

            return View(list.ToList());
        }

        //khởi tạo form 
        [HttpGet]
        public ActionResult Themsanpham()
        {
            // Load danh sách category để dropdown
            ViewBag.Categories = db.Categories.ToList(); //Lấy danh sách Category từ database➡️ Đưa danh sách đó ra View để hiển thị dropdown
            return View();
        }

        //nhập dữ liệu cho nó xử lí - lưu lại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themsanpham(Product sp)
        {
            ViewBag.Categories = db.Categories.ToList();
            var name = sp.ProName.Trim().ToLower();
            // Kiểm tra trùng: cùng tên + cùng danh mục + cùng màu + cùng dung lượng
            bool exists = db.Products.Any(p => p.ProName.Trim().ToLower() == sp.ProName.Trim().ToLower() && p.CatID == sp.CatID);
            // Nếu Product có thêm ColorID, DungLuongID thì thêm vào kiểm tra:
            // && p.ColorID == sp.ColorID
            // && p.DungLuongID == sp.DungLuongID


            if (exists)
            {
                ModelState.AddModelError("", "Sản phẩm này đã tồn tại với các thông tin giống hệt, vui lòng nhập khác.");
            }

            if (ModelState.IsValid)
            {
               
                if (sp.ImagePath != null && sp.ImagePath.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(sp.ImagePath.FileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string folder = Server.MapPath("~/hinh/");
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder); // tạo folder nếu chưa tồn tại
                        }
                        string path = Path.Combine(folder, fileName);
                        sp.ImagePath.SaveAs(path);
                        sp.ProImage = "/hinh/" + fileName;
                    }
                }

                db.Products.Add(sp);
                db.SaveChanges();
                return RedirectToAction("sanpham", "Product", new { area = "Admin" });
            }

            return View(sp);
        }



        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            var sp = db.Products.Find(id);
            if (sp == null) return HttpNotFound();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoasanpham(int id, Product sp)
        {
            sp = db.Products.Find(id);
            if (sp == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                var details = db.ProductDetails.Where(d => d.ProID == id).ToList();
                db.ProductDetails.RemoveRange(details);

                db.Products.Remove(sp);
                db.SaveChanges();
            }
            return RedirectToAction("sanpham");
        }



        [HttpGet]
        public ActionResult Suasanpham(int id)
        {

            var product = db.Products.Find(id);//trả về đối tượng category tìm kiếm theo id
            ViewBag.Categories = new SelectList(db.Categories, "CatID", "CatName", product.CatID);
            return View(product);
        }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Suasanpham(int id, Product sp)
            {
                var product = db.Products.Find(id);
                if (product == null) return HttpNotFound();
                // Kiểm tra trùng tên trong cùng category, bỏ chính nó
                var name = sp.ProName.Trim().ToLower();
                        bool exists = db.Products.Any(p => p.ProName.Trim().ToLower() == sp.ProName.Trim().ToLower() &&
                                                           p.CatID == sp.CatID && p.ProID != id);
            // bỏ chính nó && p.ColorID == sp.ColorID && p.DungLuongID == sp.DungLuongID


            if (exists)
                {
                    ModelState.AddModelError("", "Sản phẩm này đã tồn tại với các thông tin giống hệt, vui lòng nhập khác.");
                }

            product.ProName = sp.ProName;
            product.ProDescription = sp.ProDescription;
            product.CatID = sp.CatID;

            if (sp.ImagePath != null) // nếu chọn file mới
            {
                string fileName = Path.GetFileName(sp.ImagePath.FileName);
                string path = Path.Combine(Server.MapPath("~/hinh/"), fileName);
                sp.ImagePath.SaveAs(path);

                product.ProImage = "/hinh/" + fileName; // cập nhật DB
            }

            db.SaveChanges();
            return RedirectToAction("sanpham", "Product", new { area = "Admin" });
        }



        public ActionResult Chitietsanpham(int id)
        {
            var sp = db.Products.Find(id);
            if (sp == null)
                return HttpNotFound();

            return View(sp);

        }
    }
}