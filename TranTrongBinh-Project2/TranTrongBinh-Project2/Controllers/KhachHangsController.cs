using System;
using System.Linq;
using System.Web.Mvc;
using TranTrongBinh_Project2.Models;

namespace TranTrongBinh_Project2.Controllers
{
    public class KhachHangsController : Controller
    {
        private BanHangDBEntities2 db = new BanHangDBEntities2();

        // GET: KhachHangs/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: KhachHangs/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Ho,Ten,Email,SoDienThoai,DiaChi,Role")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.KhachHangs.Any(k => k.Email == khachHang.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(khachHang);
                }

                
                khachHang.Role = "user"; 

                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: KhachHangs/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string soDienThoai)
        {
            var user = db.KhachHangs.FirstOrDefault(k => k.Email == email && k.SoDienThoai == soDienThoai);
            if (user != null)
            {
                Session["User"] = user;

                
                if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    
                    return RedirectToAction("Admin", "DonHangs");
                }
                else if (user.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                {
                   
                    return RedirectToAction("Index", "Home");
                }
            }

            // Nếu không tìm thấy người dùng, thông báo lỗi
            ModelState.AddModelError("", "Email hoặc số điện thoại không đúng.");
            return View();
        }


        public ActionResult Logout()
        {
            
            Session["User"] = null;
            return RedirectToAction("Login");
        }

    }
}
