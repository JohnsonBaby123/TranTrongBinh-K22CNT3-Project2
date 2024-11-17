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
    public class ThanhToansController : Controller
    {
        private BanHangDBEntities2 db = new BanHangDBEntities2();

        // GET: ThanhToans
        public ActionResult Index()
        {
            var thanhToans = db.ThanhToans.Include(t => t.DonHang);
            return View(thanhToans.ToList());
        }

        // GET: ThanhToans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public ActionResult Create(int? MaDonHang)
        {
            if (MaDonHang == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var donHang = db.DonHangs.Find(MaDonHang);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            decimal tongTien = 0;
            foreach (var chiTiet in donHang.ChiTietDonHangs)
            {
                tongTien += chiTiet.SanPham.Gia * chiTiet.SoLuong;
            }

            // Truyền thông tin đơn hàng và tổng tiền vào ViewBag
            ViewBag.TongTien = tongTien;
            ViewBag.MaDonHang = donHang.MaDonHang;

            // Tạo danh sách phương thức thanh toán và chuyển thành SelectList
            List<SelectListItem> paymentMethods = new List<SelectListItem>
    {
        new SelectListItem { Text = "Tiền mặt", Value = "Cash" },
        new SelectListItem { Text = "Chuyển khoản", Value = "Transfer" },
        new SelectListItem { Text = "Thẻ tín dụng", Value = "CreditCard" }
    };
            ViewBag.PhuongThucThanhToan = new SelectList(paymentMethods, "Value", "Text");

            return View();
        }

        // POST: ThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThanhToan,MaDonHang,PhuongThucThanhToan,SoTienThanhToan,NgayThanhToan")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                db.ThanhToans.Add(thanhToan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // GET: ThanhToans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThanhToan,MaDonHang,PhuongThucThanhToan,SoTienThanhToan,NgayThanhToan")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thanhToan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            db.ThanhToans.Remove(thanhToan);
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
