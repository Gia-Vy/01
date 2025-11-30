using Phone_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Phone_web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        WebEntities db = new WebEntities();
        // GET: Admin/User
        public ActionResult taikhoan()
        {
            var list = db.Users.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult themtk()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult themtk(User user)
        {
            if (ModelState.IsValid)
            {
             
                user.CreateDate = DateTime.Now;;
                user.LastLogin = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("taikhoan");
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult suatk(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult suatk(User user)
        {
            if (ModelState.IsValid)
            {
                var u = db.Users.Find(user.UserID);
                if (u == null) return HttpNotFound();

                u.UserName = user.UserName;
                u.Email = user.Email;
                u.Role = user.Role;
                u.Password = user.Password;

                u.CreateDate = u.CreateDate;
                u.LastLogin = u.LastLogin;

                db.SaveChanges();
                return RedirectToAction("taikhoan");
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult xoatk(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xoatk (int id,User user)
        {
            user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("taikhoan");
        }

        public ActionResult chitiettk(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();


            return View(user);
        }

    }
    
}