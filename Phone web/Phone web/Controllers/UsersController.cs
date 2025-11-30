using Phone_web.Models;
using Phone_web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Phone_web.Controllers
{
    public class UsersController : Controller
    {
        private WebEntities db = new WebEntities();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập tồn tại?
                var existingUser = db.Users.SingleOrDefault(u => u.UserName == model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                // Tạo User
                var user = new User
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Role = "Customer",
                    Email = model.CusEmail,
                    CreateDate = DateTime.Now,
                    CreateBy = model.UserName
                   
                };
                db.Users.Add(user);

                // Tạo Customer
                var customer = new Customer
                {
                    CusName = model.CusName,
                    CusEmail = model.CusEmail,
                    CusPhone = model.CusPhone,
                    CusPassword = model.Password,
                    Role = "Customer",
                    CreateDate = DateTime.Now,
                    CreateBy = model.UserName,
                    LastLogin = DateTime.Now // 👈 thêm dòng này


                };
                db.Customers.Add(customer);

                db.SaveChanges();
                return RedirectToAction("Login", "Users");
            }

            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == model.Username
                    && u.Password == model.Password
                    && u.Role == "Customer");
                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào session
                    Session["UserName"] = user.UserName;
                    Session["Role"] = user.Role;

                    // lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.UserName, false);

                    return RedirectToAction("ProfileKH", "Users");

                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }




       
        public ActionResult ProfileKH()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Login");

            string username = Session["UserName"].ToString();

            // Lấy user theo UserName
            var user = db.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null) return HttpNotFound();

            // Lấy customer theo Email (đã lưu khi Register)
            var customer = db.Customers.SingleOrDefault(c => c.CusEmail == user.Email);
            if (customer == null) return HttpNotFound();

            // Truyền customer sang view
            return View(customer);
        }




        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}