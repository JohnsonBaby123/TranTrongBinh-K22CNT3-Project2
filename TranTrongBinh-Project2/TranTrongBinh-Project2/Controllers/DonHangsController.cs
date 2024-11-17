using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TranTrongBinh_Project2.Models;

namespace TranTrongBinh_Project2.Controllers
{
    public class DonHangsController : Controller
    {
        private BanHangDBEntities2 db = new BanHangDBEntities2();
        public ActionResult Admin()
        {
            
            var user = Session["User"] as KhachHang;
            if (user == null || user.Role != "Admin")
            {
                return RedirectToAction("Login", "KhachHangs");
            }

            // Hiển thị nội dung trang quản lý Admin
            return View();
        }
        // GET: DonHangs
        public ActionResult Index()
        {
            var donHangs = db.DonHangs.Include(d => d.KhachHang).ToList();

            return View(donHangs.ToList());
        }

        // GET: DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            // Lấy danh sách khách hàng và sản phẩm để hiển thị trên form đặt hàng
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "Ho");
            ViewBag.SanPham = new SelectList(db.SanPhams, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: DonHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang, SanPhamIds, SoLuong, PhuongThucThanhToan")] DonHang donHang, List<int> SanPhamIds, List<int> SoLuong)
        {
            if (ModelState.IsValid)
            {
                // Thêm đơn hàng vào database
                donHang.NgayDat = DateTime.Now;
                donHang.TongTien = 0; // Tạm thời set là 0, sau đó sẽ tính tổng tiền

                db.DonHangs.Add(donHang);
                db.SaveChanges(); // Lưu đơn hàng vào database để lấy MaDonHang

                // Lưu thông tin chi tiết đơn hàng
                decimal tongTien = 0;
                for (int i = 0; i < SanPhamIds.Count; i++)
                {
                    var sanPham = db.SanPhams.Find(SanPhamIds[i]);
                    var chiTiet = new ChiTietDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaSanPham = SanPhamIds[i],
                        SoLuong = SoLuong[i],
                        Gia = sanPham.Gia
                    };
                    db.ChiTietDonHangs.Add(chiTiet);
                    tongTien += sanPham.Gia * SoLuong[i];
                }

                // Cập nhật tổng tiền cho đơn hàng
                donHang.TongTien = tongTien;
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges(); // Lưu thông tin tổng tiền

                return RedirectToAction("Index");
            }

            // Nếu không hợp lệ, quay lại trang Create
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "Ho", donHang.MaKhachHang);
            ViewBag.SanPham = new SelectList(db.SanPhams, "MaSanPham", "TenSanPham");
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "Ho", donHang.MaKhachHang);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,MaKhachHang,NgayDat,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "Ho", donHang.MaKhachHang);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
