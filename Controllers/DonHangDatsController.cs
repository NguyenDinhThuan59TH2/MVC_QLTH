using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FreeTime1.Models;

namespace FreeTime1.Controllers
{
    public class DonHangDatsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index()
        {
            var donHangDats = db.DonHangDats.Include(d => d.KhachHang);
            return View(donHangDats.ToList());
        }
        public ActionResult Orders()
        {
            KhachHang skhachHang = Session["khachHang"] as KhachHang;
            if (skhachHang == null || db.KhachHangs.Where(d => d.MaKH == skhachHang.MaKH).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            DonHangDat donHangDat = db.DonHangDats.Where(d => d.MaKH == skhachHang.MaKH && d.TrangThai == "Đang đặt").Include(d => d.HangDonHangDats).FirstOrDefault();
            if (donHangDat == null || donHangDat.HangDonHangDats.Count() == 0)
            {
                return RedirectToAction("Index", "Home", new { ThongBao = "Đơn hàng trống" });
            }
            foreach (HangDonHangDat hangDonHangDat in donHangDat.HangDonHangDats)
            {
                Hang hang = db.Hangs.Where(d => d.MaH == hangDonHangDat.MaH).Include(d => d.MauHang).FirstOrDefault();
                hangDonHangDat.Hang = hang;
            }
            ViewBag.CartBackStore = true;
            return View("Order", donHangDat);
        }
        public ActionResult History(string MaKH)
        {
            KhachHang skhachHang = Session["khachHang"] as KhachHang;
            if (skhachHang == null || MaKH != skhachHang.MaKH || db.KhachHangs.Where(d => d.MaKH == skhachHang.MaKH).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            System.Diagnostics.Debug.WriteLine("MaKH", MaKH);
            var donHangDats = db.DonHangDats.Where(d =>
                d.MaKH == MaKH &&
                (d.TrangThai == "Đã đặt" || d.TrangThai == "Đang giao" || d.TrangThai == "Đã thanh toán")
            ).Include(d => d.HangDonHangDats).ToList();
            if (donHangDats.Count() <= 0) return RedirectToAction("Index", "Home", new { ThongBao = "Chưa có lịch sử" });
            foreach (DonHangDat donHangDat in donHangDats)
            {
                if (donHangDat != null)
                {
                    foreach (HangDonHangDat hangDonHangDat in donHangDat.HangDonHangDats)
                    {
                        Hang hang = db.Hangs.Where(d => d.MaH == hangDonHangDat.MaH).Include(d => d.MauHang).FirstOrDefault();
                        hangDonHangDat.Hang = hang;
                    }
                }
            }
            ViewBag.HistoryBackStore = true;
            return View("History", donHangDats);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangDat donHangDat = db.DonHangDats.Find(id);
            if (donHangDat == null)
            {
                return HttpNotFound();
            }
            return View(donHangDat);
        }
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDHD,MaKH,NgayDat,TrangThai")] DonHangDat donHangDat)
        {
            if (ModelState.IsValid)
            {
                db.DonHangDats.Add(donHangDat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHangDat.MaKH);
            return View(donHangDat);
        }

        [HttpPost]
        public ActionResult DeleteOrder(string MaDHD, string MaH)
        {
            KhachHang skhachHang = Session["khachHang"] as KhachHang;
            if (skhachHang == null || db.KhachHangs.Where(d => d.MaKH == skhachHang.MaKH && d.DaXoa == false).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            HangDonHangDat hangDonHangDat = db.HangDonHangDats.Where(d => d.MaDHD == MaDHD && d.MaH == MaH).FirstOrDefault();
            if (hangDonHangDat != null)
            {
                db.HangDonHangDats.Remove(hangDonHangDat);
                db.SaveChanges();
            }
            return RedirectToAction("Orders");
        }

        [HttpPost]
        public ActionResult OrderDocument(string MaKH)
        {
            KhachHang khachHang = db.KhachHangs.Where(d => d.MaKH == MaKH && d.DaXoa == false).FirstOrDefault();
            if (khachHang == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            DonHangDat donHangDat = db.DonHangDats.Where(d => d.TrangThai == "Đang đặt" && d.MaKH == MaKH).FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("MaKH", MaKH);
            if (donHangDat == null)
            {
                return RedirectToAction("Index", "Home");
            }
            donHangDat.TrangThai = "Đã đặt";
            donHangDat.NgayDat = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index", "Home", new { ThongBao = "Đặt thành công" });
        }
        [HttpPost]
        public ActionResult OrderStock(string MaKH, string MaH, string SoLuong)
        {
            KhachHang skhachHang = Session["khachHang"] as KhachHang;
            if (skhachHang == null || db.KhachHangs.Where(d => d.MaKH == skhachHang.MaKH && d.DaXoa == false).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            if (SoLuong == "")
            {
                ViewBag.Loi = "Chưa nhập số lượng";
            } else
            {
                int sl = int.Parse(SoLuong);
                Hang hang = db.Hangs.Where(d => d.MaH == MaH).FirstOrDefault();
                DonHangDat donHangDat = db.DonHangDats.Where(d => d.MaKH == MaKH && d.TrangThai == "Đang đặt").Include(d => d.HangDonHangDats).FirstOrDefault();
                if (donHangDat != null)
                {
                    HangDonHangDat hangDonHangDat = db.HangDonHangDats.Where(d => d.MaDHD == donHangDat.MaDHD && d.MaH == MaH).FirstOrDefault();
                    if (hangDonHangDat != null)
                    {
                        hangDonHangDat.SoLuong += sl;
                    } else
                    {
                        HangDonHangDat hangDonHangDatMoi = new HangDonHangDat();
                        hangDonHangDatMoi.MaDHD = donHangDat.MaDHD;
                        hangDonHangDatMoi.MaH = MaH;
                        hangDonHangDatMoi.SoLuong = sl;
                        db.HangDonHangDats.Add(hangDonHangDatMoi);
                    }
                    ViewBag.SoLuongHangDat = donHangDat.HangDonHangDats.Count() + 1;
                } else
                {
                    DonHangDat donHangDatMoi = new DonHangDat();
                    int count = db.DonHangDats.Count() + 1;
                    donHangDatMoi.MaDHD = "DHD" + count.ToString();
                    donHangDatMoi.MaKH = MaKH;
                    donHangDatMoi.NgayBatDau = DateTime.Now;
                    donHangDatMoi.TrangThai = "Đang đặt";

                    HangDonHangDat hangDonHangDatMoi = new HangDonHangDat();
                    hangDonHangDatMoi.MaDHD = donHangDatMoi.MaDHD;
                    hangDonHangDatMoi.MaH = MaH;
                    hangDonHangDatMoi.SoLuong = sl;
                    db.DonHangDats.Add(donHangDatMoi);
                    db.HangDonHangDats.Add(hangDonHangDatMoi);
                    ViewBag.SoLuongHangDat = 1;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangDat donHangDat = db.DonHangDats.Find(id);
            if (donHangDat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHangDat.MaKH);
            return View(donHangDat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDHD,MaKH,NgayDat,TrangThai")] DonHangDat donHangDat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHangDat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHangDat.MaKH);
            return View(donHangDat);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangDat donHangDat = db.DonHangDats.Find(id);
            if (donHangDat == null)
            {
                return HttpNotFound();
            }
            return View(donHangDat);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonHangDat donHangDat = db.DonHangDats.Find(id);
            db.DonHangDats.Remove(donHangDat);
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
