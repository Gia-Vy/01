using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Phone_web.Models;

namespace Phone_web.Controllers
{
    public class CustomerHomeController : Controller
    {
        private WebEntities db = new WebEntities();

        // Trang chủ khách hàng
        public ActionResult trangchuKH()
        {
            var products = db.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProID)
                .ToList();

            // Nhóm sản phẩm theo danh mục, lấy 4 sản phẩm mỗi nhóm
            var groupedProducts = products
                .GroupBy(p => p.CatID)
                .ToDictionary(g => g.Key, g => g.Take(4).ToList());

            ViewBag.Categories = db.Categories.ToList();

            return View(groupedProducts); // View nhận Dictionary<int, List<Product>>
        }

        // Chi tiết sản phẩm
        public ActionResult ChiTietSP(int id)
        {
            var product = db.Products
                            .Include(p => p.ProductDetails.Select(pd => pd.Color))
                            .Include(p => p.ProductDetails.Select(pd => pd.DungLuong))
                            .Include(p => p.ProductImages)
                            .FirstOrDefault(p => p.ProID == id);

            if (product == null)
                return HttpNotFound();

            var vm = new ProductDetailViewModel
            {
                ProID = product.ProID,
                ProName = product.ProName,
                ProImage = product.ProImage,
                ProDescription = product.ProDescription,
                Images = product.ProductImages?.Select(i => i.ImagePath).ToList() ?? new List<string>(),
                Versions = product.ProductDetails.Select(pd => new ProductVersionViewModel
                {
                    ProDeID = pd.ProDeID,
                    ColorName = pd.Color?.ColorName ?? "Chưa có",
                    DungLuongName = pd.DungLuong?.DungLuongName ?? "Chưa có",
                    Price = pd.Price
                }).ToList()
            };

            return View(vm); // View nhận ProductDetailViewModel
        }

        // Sản phẩm theo danh mục
        public ActionResult Category(int id)
        {
            var products = db.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.Category)
                .Where(p => p.CatID == id)
                .ToList();

            var category = db.Categories.Find(id);
            ViewBag.CategoryName = category?.CatName ?? "Danh mục";

            return View(products); // View nhận IEnumerable<Product>
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return RedirectToAction("trangchuKH"); // Về trang chủ nếu không nhập
            }

            // Chuẩn bị parse giá nếu keyword là số
            decimal priceFilter;
            bool isPrice = decimal.TryParse(keyword, out priceFilter);

            var productsQuery = db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                .Where(p =>
                    // Tìm theo tên sản phẩm
                    p.ProName.Contains(keyword) ||

                    // Tìm theo mô tả sản phẩm
                    (!string.IsNullOrEmpty(p.ProDescription) && p.ProDescription.Contains(keyword)) ||

                    // Tìm theo tên danh mục (iPhone, iPad, Mac…)
                    (p.Category != null && p.Category.CatName.Contains(keyword)) ||

                    // Tìm theo giá nếu keyword là số
                    (isPrice && p.ProductDetails.Any(pd => pd.Price == priceFilter))
                );

            var products = productsQuery.ToList();

            return View(products);
        }

    }
}