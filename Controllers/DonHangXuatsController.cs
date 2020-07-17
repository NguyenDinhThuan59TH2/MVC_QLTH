using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FreeTime1.Models;

namespace FreeTime1.Controllers
{
    public class DonHangXuatsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: DonHangXuats
        public ActionResult Index(string XoaThanhCong = null)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) 
                return RedirectToAction("Index", "Login");
            var donHangXuats = db.DonHangXuats.Where(d => d.DaXoa == false).Include(d => d.KhachHang);
            decimal TongGiaTriXuat = 0;
            foreach (DonHangXuat donHangXuat in donHangXuats)
            {
                donHangXuat.TongDonHang = 0;
                var hangDonHangXuats = db.HangDonHangXuats.Where(d => d.MaDHX == donHangXuat.MaDHX);
                foreach (HangDonHangXuat hangDonHangXuat in hangDonHangXuats)
                {
                    Hang hang = db.Hangs.Where(d => d.MaH == hangDonHangXuat.MaH).First();
                    donHangXuat.TongDonHang += hangDonHangXuat.SoLuong * hang.GiaBan;
                }
                // tinh giam gia
                if (donHangXuat.KieuGiamGia != "" && donHangXuat.GiamGia != null)
                {
                    if (donHangXuat.KieuGiamGia == "VNĐ")
                    {
                        donHangXuat.TongDonHang -= Math.Round(decimal.Parse(donHangXuat.GiamGia), 0);
                    }
                    else if (donHangXuat.KieuGiamGia == "%")
                    {
                        donHangXuat.TongDonHang -= Math.Round(donHangXuat.TongDonHang / 100 * decimal.Parse(donHangXuat.GiamGia),0);
                        
                    }
                }
                TongGiaTriXuat += donHangXuat.TongDonHang;
            }
            ViewBag.TongGiaTriXuat = TongGiaTriXuat;
            ViewBag.XoaThanhCong = XoaThanhCong;
            return View(donHangXuats.ToList());
        }

        public ActionResult EditDocument(string MaDHX)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            DonHangXuat donHangXuat = db.DonHangXuats.Where(d => d.MaDHX == MaDHX && d.DaXoa == false && d.DaDuyet == false).FirstOrDefault();
            if (donHangXuat == null) return RedirectToAction("Index");
            donHangXuat.KhachHang = db.KhachHangs.Where(d => d.MaKH == donHangXuat.MaKH).FirstOrDefault();
            donHangXuat.HangDonHangXuats = db.HangDonHangXuats.Where(d => d.MaDHX == donHangXuat.MaDHX).ToList();
            decimal TongDonHang = 0;
            foreach (var hangDonHangXuat in donHangXuat.HangDonHangXuats)
            {
                hangDonHangXuat.Hang = db.Hangs.Where(d => d.MaH == hangDonHangXuat.MaH).FirstOrDefault();
                hangDonHangXuat.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == hangDonHangXuat.Hang.MaMH).FirstOrDefault();
                TongDonHang += hangDonHangXuat.SoLuong * hangDonHangXuat.Hang.GiaBan;
            }
            if (donHangXuat.KieuGiamGia == "%" && donHangXuat.GiamGia != null)
            {
                TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangXuat.GiamGia);
            }
            else if (donHangXuat.KieuGiamGia == "VNĐ" && donHangXuat.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangXuat.GiamGia);
            }
            ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Where(d => d.SoLuong > 0).Include(d => d.MauHang).ToList();
            return View("Create", donHangXuat);
        }

        // GET: DonHangXuats/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hangDonHangXuats = db.HangDonHangXuats.Where(hangDonHangXuat => hangDonHangXuat.MaDHX == id).Include(hangDonHangNhap => hangDonHangNhap.DonHangXuat);
            decimal TongDonhang = 0;
            foreach (var hangDonHangXuat in hangDonHangXuats)
            {
                var KhachHang = db.KhachHangs.Where(d => d.MaKH == hangDonHangXuat.DonHangXuat.MaKH).FirstOrDefault();
                var Hang = db.Hangs.Where(d => d.MaH == hangDonHangXuat.MaH).FirstOrDefault();
                var MauHang = db.MauHangs.Where(d => d.MaMH == Hang.MaMH).FirstOrDefault();
                hangDonHangXuat.DonHangXuat.KhachHang = KhachHang;
                hangDonHangXuat.Hang = Hang;
                hangDonHangXuat.Hang.MauHang = MauHang;
                TongDonhang += hangDonHangXuat.SoLuong * hangDonHangXuat.Hang.GiaBan;
            }
            if (hangDonHangXuats == null)
            {
                return HttpNotFound();
            }
            DonHangXuat donHangXuat = db.DonHangXuats.Where(d => d.MaDHX == id).Include(d => d.KhachHang).FirstOrDefault();
            if (donHangXuat.KieuGiamGia != "" && donHangXuat.GiamGia != null)
            {
                if (donHangXuat.KieuGiamGia == "VNĐ")
                {
                    TongDonhang -= decimal.Parse(donHangXuat.GiamGia);
                }
                else if (donHangXuat.KieuGiamGia == "%")
                {
                    TongDonhang -= TongDonhang / 100 * decimal.Parse(donHangXuat.GiamGia);
                }
                ViewBag.GiamGia = String.Format("{0:n0}", decimal.Parse(donHangXuat.GiamGia));
                ViewBag.KieuGiamGia = donHangXuat.KieuGiamGia;
            }
            ViewBag.TongDonhang = String.Format("{0:n0}", TongDonhang) + "VNĐ";
            ViewBag.MaDHX = donHangXuat.MaDHX;
            ViewBag.TenKH = donHangXuat.KhachHang.HoTen;
            ViewBag.NgayXuat = donHangXuat.NgayXuat;
            return View(hangDonHangXuats);
        }

        // GET: DonHangXuats/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            ViewBag.MaKH = new SelectList(db.KhachHangs.Where(d => d.DaXoa == false), "MaKH", "HoTen");
            return View();
        }
        public ActionResult TimKiem(string MaDHX, string TenKH, string NgayXuatBD, string NgayXuatKT, string KieuGiamGia)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            bool TimKiemNgayNhap = NgayXuatBD != "" && NgayXuatKT != "" ? true : false;
            DateTime NgayXuatBDDate = new DateTime();
            DateTime NgayXuatKTDate = new DateTime();
            if (TimKiemNgayNhap)
            {
                NgayXuatBDDate = DateTime.ParseExact(NgayXuatBD, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                NgayXuatKTDate = DateTime.ParseExact(NgayXuatKT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            var DonHangXuats = db.DonHangXuats.Include(DonHangNhap => DonHangNhap.KhachHang).Where(d =>
                (MaDHX == "" || d.MaDHX.Contains(MaDHX)) &&
                (TenKH == "" || d.KhachHang.HoTen.Contains(TenKH)) &&
                (KieuGiamGia == null || d.KieuGiamGia.Contains(KieuGiamGia)) &&
                ((TimKiemNgayNhap && NgayXuatBDDate <= d.NgayXuat && d.NgayXuat <= NgayXuatKTDate) || !TimKiemNgayNhap) &&
                d.DaXoa == false
            );
            ViewBag.MaDHX = MaDHX;
            ViewBag.TenKH = TenKH;
            ViewBag.NgayNhapBD = NgayXuatBD;
            ViewBag.NgayNhapKT = NgayXuatKT;
            ViewBag.KieuGiamGia = KieuGiamGia;
            return View("Index", DonHangXuats);
        }

        // POST: DonHangXuats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocument([Bind(Include = "MaKH,NgayXuat,GiamGia,KieuGiamGia")] DonHangXuat donHangXuat)
        {
            if (ModelState.IsValid)
            {
                int count = db.DonHangXuats.Count() + 1;
                donHangXuat.MaDHX = "DHX" + count.ToString();
                if (donHangXuat.KieuGiamGia == "True")
                {
                    donHangXuat.KieuGiamGia = "VNĐ";
                }
                else
                {
                    donHangXuat.KieuGiamGia = "%";
                }
                donHangXuat.DaXoa = false;
                db.DonHangXuats.Add(donHangXuat);
                db.SaveChanges();
                donHangXuat.KhachHang = db.KhachHangs.Where(d => d.MaKH == donHangXuat.MaKH).FirstOrDefault();
                donHangXuat.HangDonHangXuats = db.HangDonHangXuats.Where(d => d.MaDHX == donHangXuat.MaDHX).ToList();
                decimal TongDonHang = 0;
                foreach (var hangDonHangXuat in donHangXuat.HangDonHangXuats)
                {
                    hangDonHangXuat.Hang = db.Hangs.Where(d => d.MaH == hangDonHangXuat.MaH).FirstOrDefault();
                    hangDonHangXuat.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == hangDonHangXuat.Hang.MaMH).FirstOrDefault();
                    TongDonHang += hangDonHangXuat.SoLuong * hangDonHangXuat.Hang.GiaBan;
                }
                if (donHangXuat.KieuGiamGia == "%" && donHangXuat.GiamGia != null)
                {
                    TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangXuat.GiamGia);
                }
                else if (donHangXuat.KieuGiamGia == "VNĐ" && donHangXuat.GiamGia != null)
                {
                    TongDonHang -= decimal.Parse(donHangXuat.GiamGia);
                }
                ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
                ViewBag.MauHangs = db.MauHangs.ToList();
                ViewBag.Hangs = db.Hangs.Where(d => d.SoLuong > 0).Include(d => d.MauHang).ToList();
                return View("Create", donHangXuat);
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            return View("Create");
        }
        public ActionResult AddInStock(string MaDHX, string MaH, string SoLuong)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            var donHangXuat = db.DonHangXuats.Where(d => d.MaDHX == MaDHX && d.DaDuyet == false && d.DaXoa == false).FirstOrDefault();
            Hang hang = db.Hangs.Single(d => d.MaH == MaH);
            bool loi = false;
            if (SoLuong == "")
            {
                ViewBag.Loi = "Chưa nhập số lượng";
                loi = true;
            }
            else if (int.Parse(SoLuong) <= 0)
            {
                ViewBag.Loi = "Số lượng phải lớn hơn 0";
                loi = true;
            }
            else if (hang.SoLuong < int.Parse(SoLuong))
            {
                ViewBag.errCount = "Số lượng vượt quá số lượng có trong kho";
            }
            else
            {
                var HDHX = db.HangDonHangXuats.Where(d => d.MaDHX == MaDHX && d.MaH == MaH).FirstOrDefault();
                if (HDHX != null)
                {
                    HDHX.SoLuong += int.Parse(SoLuong);
                }
                else
                {
                    HangDonHangXuat hangDonHangXuat = new HangDonHangXuat();
                    hangDonHangXuat.SoLuong = int.Parse(SoLuong);
                    hangDonHangXuat.MaH = MaH;
                    hangDonHangXuat.MaDHX = donHangXuat.MaDHX;
                    db.HangDonHangXuats.Add(hangDonHangXuat);
                }
                hang.SoLuong -= int.Parse(SoLuong);
                db.SaveChanges();
            }
            donHangXuat.KhachHang = db.KhachHangs.Where(d => d.MaKH == donHangXuat.MaKH).FirstOrDefault();
            donHangXuat.HangDonHangXuats = db.HangDonHangXuats.Where(d => d.MaDHX == donHangXuat.MaDHX).ToList();
            decimal TongDonHang = 0;
            foreach (var HangDonHangXuat in donHangXuat.HangDonHangXuats)
            {
                HangDonHangXuat.Hang = db.Hangs.Where(d => d.MaH == HangDonHangXuat.MaH).FirstOrDefault();
                HangDonHangXuat.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == HangDonHangXuat.Hang.MaMH).FirstOrDefault();
                TongDonHang = HangDonHangXuat.SoLuong * HangDonHangXuat.Hang.GiaBan;
            }
            if (donHangXuat.KieuGiamGia == "%" && donHangXuat.GiamGia != null)
            {
                TongDonHang = TongDonHang * ((100 - decimal.Parse(donHangXuat.GiamGia))/100);
            }
            else if (donHangXuat.KieuGiamGia == "VNĐ" && donHangXuat.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangXuat.GiamGia);
            }
            ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Where(d => d.SoLuong > 0).Include(d => d.MauHang).ToList();
            return View("Create", donHangXuat);
        }
        public ActionResult DeleteInStock(string MaDHX, string MaH)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            var donHangXuat = db.DonHangXuats.Where(d => d.MaDHX == MaDHX && d.DaDuyet == false && d.DaXoa == false).FirstOrDefault();
            Hang hang = db.Hangs.Single(d => d.MaH == MaH);
            if (hang != null) {
                var HDHX = db.HangDonHangXuats.Where(d => d.MaDHX == MaDHX && d.MaH == MaH).FirstOrDefault();
                if (HDHX != null)
                {
                    db.HangDonHangXuats.Remove(HDHX);
                    hang.SoLuong += HDHX.SoLuong;
                    db.SaveChanges();
                }
            }
            donHangXuat.KhachHang = db.KhachHangs.Where(d => d.MaKH == donHangXuat.MaKH).FirstOrDefault();
            donHangXuat.HangDonHangXuats = db.HangDonHangXuats.Where(d => d.MaDHX == donHangXuat.MaDHX).ToList();
            decimal TongDonHang = 0;
            foreach (var HangDonHangXuat in donHangXuat.HangDonHangXuats)
            {
                HangDonHangXuat.Hang = db.Hangs.Where(d => d.MaH == HangDonHangXuat.MaH).FirstOrDefault();
                HangDonHangXuat.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == HangDonHangXuat.Hang.MaMH).FirstOrDefault();
                TongDonHang = HangDonHangXuat.SoLuong * HangDonHangXuat.Hang.GiaBan;
            }
            if (donHangXuat.KieuGiamGia == "%" && donHangXuat.GiamGia != null)
            {
                TongDonHang = TongDonHang * ((100 - decimal.Parse(donHangXuat.GiamGia)) / 100);
            }
            else if (donHangXuat.KieuGiamGia == "VNĐ" && donHangXuat.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangXuat.GiamGia);
            }
            ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Where(d => d.SoLuong > 0).Include(d => d.MauHang).ToList();
            return View("Create", donHangXuat);
        }
        public ActionResult DeleteDocument(string MaDHX)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            DonHangXuat donHangXuat = db.DonHangXuats.Where(d => d.MaDHX == MaDHX && d.DaDuyet == false && d.DaXoa == false).FirstOrDefault();
            if (donHangXuat != null)
            {
                donHangXuat.DaXoa = true;
                db.SaveChanges();
            }
            var donHangXuats = db.DonHangXuats.Include(d => d.KhachHang);
            return RedirectToAction("Index", new { XoaThanhCong = "Xóa đơn hàng " + MaDHX + " thành công" });
        }

        // GET: DonHangXuats/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangXuat donHangXuat = db.DonHangXuats.Find(id);
            if (donHangXuat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHangXuat.MaKH);
            return View(donHangXuat);
        }

        // POST: DonHangXuats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDHX,MaKH,NgayXuat,GiamGia,KieuGiamGia")] DonHangXuat donHangXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHangXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHangXuat.MaKH);
            return View(donHangXuat);
        }

        // GET: DonHangXuats/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangXuat donHangXuat = db.DonHangXuats.Find(id);
            if (donHangXuat == null)
            {
                return HttpNotFound();
            }
            return View(donHangXuat);
        }

        // POST: DonHangXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonHangXuat donHangXuat = db.DonHangXuats.Find(id);
            db.DonHangXuats.Remove(donHangXuat);
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
