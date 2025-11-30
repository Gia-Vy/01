using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Areas.Admin.Controllers
{
    public class ThongkeController : Controller
    {
        WebEntities db = new WebEntities();
        // GET: Admin/Thongke
        public ActionResult thongke()
        {
            
           
                // --- 1. Thống kê số lượng ---
                ViewBag.TotalCategory = db.Categories.Count();
                ViewBag.TotalProduct = db.Products.Count();
                ViewBag.TotalProductDetail = db.ProductDetails.Count();
                ViewBag.TotalCustomer = db.Customers.Count();
                ViewBag.TotalUser = db.Users.Count();
                ViewBag.TotalOrder = db.Orders.Count();
                ViewBag.TotalOrderDetail = db.OrderDetails.Count();

                // --- 2. Tình trạng đơn hàng ---
                ViewBag.OrderPending = db.Orders.Count(o => o.OrderStatus == "Đang xử lý");
                ViewBag.OrderShipping = db.Orders.Count(o => o.OrderStatus == "Đang giao");
                ViewBag.OrderDone = db.Orders.Count(o => o.OrderStatus == "Hoàn tất");
                ViewBag.OrderCancel = db.Orders.Count(o => o.OrderStatus == "Hủy");



            return View();
            }
        }
    }


    
