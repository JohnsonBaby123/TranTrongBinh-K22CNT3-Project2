using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TranTrongBinh_Project2.Models;

namespace TranTrongBinh_Project2.Controllers
{
    public class SanPhamsController : Controller
    {
        private BanHangDBEntities2 db = new BanHangDBEntities2();

        // GET: SanPhams/Order
        public ActionResult Order()
        {
            // Get all products from the database
            var products = db.SanPhams.ToList();
            return View(products);
        }

        // GET: SanPhams/CreateOrder/5
        public ActionResult CreateOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve product details based on product ID
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Pass the product data directly to the view
            return View(sanPham);
        }

        // POST: SanPhams/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(int id, int quantity)
        {
            // Kiểm tra xem khách hàng đã đăng nhập hay chưa
            var user = Session["User"] as KhachHang;
            if (user == null)
            {
                // Nếu khách hàng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "KhachHangs");
            }

            // Retrieve the product based on the ID
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Validate quantity (must be greater than 0)
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Quantity must be greater than zero.");
                return View(sanPham);
            }

            // Create a new order with the logged-in user's ID
            DonHang newOrder = new DonHang
            {
                MaKhachHang = user.MaKhachHang, // Sử dụng MaKhachHang từ session
                NgayDat = DateTime.Now,
                TongTien = sanPham.Gia * quantity
            };

            db.DonHangs.Add(newOrder);
            db.SaveChanges();

            // Add order details
            ChiTietDonHang orderDetail = new ChiTietDonHang
            {
                MaDonHang = newOrder.MaDonHang,
                MaSanPham = sanPham.MaSanPham,
                SoLuong = quantity,
                Gia = sanPham.Gia
            };

            db.ChiTietDonHangs.Add(orderDetail);
            db.SaveChanges();

            // Redirect to the product list or another page after successful order creation
            return RedirectToAction("Index", "SanPhams");
        }

        // GET: SanPhams
        public ActionResult Index()
        {
            return View(db.SanPhams.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,DanhMuc,Gia,TonKho")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,DanhMuc,Gia,TonKho")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Dispose method to free resources
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
