using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Controllers
{
    public class CartController : Controller
    {
        private WebEntities db = new WebEntities();

        // Lấy giỏ hàng từ Session
        private List<Giohang> GetCart()
        {
            var cart = Session["Cart"] as List<Giohang>;
            if (cart == null)
            {
                cart = new List<Giohang>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Trang giỏ hàng
        public ActionResult Giohang()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(s => s.total);
            ViewBag.Count = cart.Sum(s => s.quantity);
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ
        public ActionResult ThemGH(int id)
        {
            var cart = GetCart();
            var productDetail = db.ProductDetails.Find(id);
            if (productDetail == null) return RedirectToAction("Giohang");

            var item = cart.FirstOrDefault(s => s.idPro == productDetail.ProDeID);

            if (item != null)
            {
                item.quantity++;
            }
            else
            {
                cart.Add(new Giohang
                {
                    idPro = productDetail.ProDeID,
                    namePro = db.Products.Find(productDetail.ProID).ProName,
                    ColorName = db.Colors.Find(productDetail.ColorID)?.ColorName,
                    DungLuongName = db.DungLuongs.Find(productDetail.DungLuongID)?.DungLuongName,
                    quantity = 1,
                    price = (decimal)productDetail.Price
                });
            }

            Session["Cart"] = cart; // ✅ cập nhật lại Session
            return RedirectToAction("Giohang");
        }

        // Xóa sản phẩm khỏi giỏ
        public ActionResult XoaGH(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.idPro == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            Session["Cart"] = cart.Any() ? cart : null;
            return RedirectToAction("Giohang");
        }

        // Tăng số lượng
        public ActionResult Tang(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(s => s.idPro == id);
            if (item != null)
            {
                item.quantity++;
            }
            Session["Cart"] = cart;
            return RedirectToAction("Giohang");
        }

        // Giảm số lượng
        public ActionResult Giam(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(s => s.idPro == id);
            if (item != null)
            {
                item.quantity--;
                if (item.quantity <= 0) cart.Remove(item);
            }
            Session["Cart"] = cart.Any() ? cart : null;
            return RedirectToAction("Giohang");
        }
        // GET: /Cart/ThanhToan
        [HttpGet]
        public ActionResult thanhtoan()
        {
            var cart = GetCart();

            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Giohang");
            }

            return View(cart); // trả về view thanh toán, model là List<Giohang>
        }
        [HttpPost]
        public ActionResult ThanhToan(string CusPhone, string DCNhanHang, string Province, string District)
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Giohang");
            }

            // Tạo Customer nếu chưa có (fix lỗi FK)
            var customer = db.Customers.FirstOrDefault(c => c.CusPhone == CusPhone);
            if (customer == null)
            {
                customer = new Customer { CusPhone = CusPhone };
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            // Ghép địa chỉ đầy đủ
            string fullAddress = $"{DCNhanHang}, {District}, {Province}";

            var order = new Order
            {
                CusPhone = CusPhone,
                AddressDelivery = fullAddress,
                TotalValue = cart.Sum(s => s.total), // ==== MỚI: tính lại tổng tiền ====
                PaymentMethod = "COD",
                PaymentStatus = "Chưa thanh toán",
                OrderStatus = "Đang vận chuyển",
                OrderDate = DateTime.Now
            };

            db.Orders.Add(order);
            db.SaveChanges();

            foreach (var item in cart)
            {
                db.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProDeID = item.idPro,
                    Quantity = item.quantity, // ==== MỚI: dùng số lượng mới ====
                    UnitPrice = item.price
                });
            }
            db.SaveChanges();

            Session["Cart"] = null;
            TempData["Success"] = "Thanh toán thành công!";
            return RedirectToAction("Giohang");
        }


        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            var cart = GetCart();
            var productDetail = db.ProductDetails.Find(id);
            if (productDetail == null) return Json(new { count = 0, total = 0 });

            var item = cart.FirstOrDefault(s => s.idPro == productDetail.ProDeID);

            if (item != null)
                item.quantity++;
            else
                cart.Add(new Giohang
                {
                    idPro = productDetail.ProDeID,
                    namePro = db.Products.Find(productDetail.ProID).ProName,
                    ColorName = db.Colors.Find(productDetail.ColorID)?.ColorName,
                    DungLuongName = db.DungLuongs.Find(productDetail.DungLuongID)?.DungLuongName,
                    quantity = 1,
                    price = (decimal)productDetail.Price,
                     ProImage = db.Products.Find(productDetail.ProID).ProImage
                });

            Session["Cart"] = cart;

            return Json(new
            {
                count = cart.Sum(s => s.quantity),
                total = cart.Sum(s => s.total)
            });
        }
        [HttpGet]
             public ActionResult lichsudonhang()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            var orders = db.Orders

                           .Include("OrderDetails.Order")   // bây giờ hợp lệ
                           .ToList();

            return View(orders);
        }
    }
    
}