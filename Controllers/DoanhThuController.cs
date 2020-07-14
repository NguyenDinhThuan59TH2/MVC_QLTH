using FreeTime1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeTime1.Controllers
{
    public class DoanhThuController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            List<DoanhThu> doanhThus = new List<DoanhThu>();
            var DonHangNhaps = db.DonHangNhaps.Include(d => d.HangDonHangNhaps).ToList();
            var DonHangXuats = db.DonHangXuats.Include(d => d.HangDonHangXuats).ToList();
            decimal TongGiaTriNhap = 0;
            decimal TongGiaTriXuat = 0;
            foreach (var DonHangNhap in DonHangNhaps)
            {
                DoanhThu doanhThu = new DoanhThu();
                doanhThu.MaDH = DonHangNhap.MaDHN;
                doanhThu.GiamGia = DonHangNhap.GiamGia;
                doanhThu.KieuGiamGia = DonHangNhap.KieuGiamGia;
                doanhThu.NgayThucHien = DonHangNhap.NgayNhap;
                doanhThu.LoaiDonHang = "Nhập";
                decimal TongDonHang = 0;
                foreach (var HangDonhangNhap in DonHangNhap.HangDonHangNhaps)
                {
                    TongDonHang += HangDonhangNhap.SoLuong * db.Hangs.Where(d => d.MaH == HangDonhangNhap.MaH).First().GiaNhap;
                }
                if (DonHangNhap.KieuGiamGia == "VNĐ" && DonHangNhap.GiamGia != null)
                {
                    TongDonHang -= decimal.Parse(DonHangNhap.GiamGia);
                } else if (DonHangNhap.KieuGiamGia == "%" && DonHangNhap.GiamGia != null)
                {
                    TongDonHang -= TongDonHang % 100 * decimal.Parse(DonHangNhap.GiamGia);
                }
                TongGiaTriNhap += TongDonHang;
                doanhThu.TongDonHang = TongDonHang;
                doanhThus.Add(doanhThu);
            }
            foreach (var DonHangXuat in DonHangXuats)
            {
                DoanhThu doanhThu = new DoanhThu();
                doanhThu.MaDH = DonHangXuat.MaDHX;
                doanhThu.GiamGia = DonHangXuat.GiamGia;
                doanhThu.KieuGiamGia = DonHangXuat.KieuGiamGia;
                doanhThu.LoaiDonHang = "Xuất";
                doanhThu.NgayThucHien = DonHangXuat.NgayXuat;
                decimal TongDonHang = 0;
                foreach (var HangDonhangXuat in DonHangXuat.HangDonHangXuats)
                {
                    TongDonHang += HangDonhangXuat.SoLuong * db.Hangs.Where(d => d.MaH == HangDonhangXuat.MaH).First().GiaNhap;
                }
                if (DonHangXuat.KieuGiamGia == "VNĐ" && DonHangXuat.GiamGia != null)
                {
                    TongDonHang -= decimal.Parse(DonHangXuat.GiamGia);
                }
                else if (DonHangXuat.KieuGiamGia == "%" && DonHangXuat.GiamGia != null)
                {
                    TongDonHang -= TongDonHang % 100 * decimal.Parse(DonHangXuat.GiamGia);
                }
                doanhThu.TongDonHang = TongDonHang;
                TongGiaTriXuat += TongDonHang;
                doanhThus.Add(doanhThu);
            }
            ViewBag.TongGiaTriNhap = TongGiaTriNhap;
            ViewBag.TongGiaTriXuat = TongGiaTriXuat;
            return View(doanhThus);
        }
        public ActionResult TimKiem(string LoaiDonHang, string GiaTriDonHangBD, string GiaTriDonHangKT, string NgayThucHienBD, string NgayThucHienKT)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            ViewBag.LoaiDonHang = LoaiDonHang;
            ViewBag.GiaTriDonHangBD = GiaTriDonHangBD;
            ViewBag.GiaTriDonHangKT = GiaTriDonHangKT;
            ViewBag.NgayThucHienBD = NgayThucHienBD;
            ViewBag.NgayThucHienKT = NgayThucHienKT;
            bool TimKiemNgayThucHien = NgayThucHienBD != "" && NgayThucHienKT != "" ? true : false;
            DateTime NgayThucHienBDDate = new DateTime();
            DateTime NgayThucHienKTDate = new DateTime();
            if (TimKiemNgayThucHien)
            {
                NgayThucHienBDDate = DateTime.ParseExact(NgayThucHienBD, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                NgayThucHienKTDate = DateTime.ParseExact(NgayThucHienKT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            bool TimKiemGiaTriDonHang = GiaTriDonHangBD != "" && GiaTriDonHangKT != "" ? true : false;
            Decimal GiaTriDonHangBDDecimal = new Decimal();
            Decimal GiaTriDonHangKTDecimal = new Decimal();
            if (TimKiemGiaTriDonHang)
            {
                GiaTriDonHangBDDecimal = decimal.Parse(GiaTriDonHangBD);
                GiaTriDonHangKTDecimal = decimal.Parse(GiaTriDonHangKT);
            }
            bool TimKiemLoaiDonHang = false;
            if (LoaiDonHang != null)
            {
                TimKiemLoaiDonHang = true;
            }
            List<DoanhThu> doanhThus = new List<DoanhThu>();
            decimal TongGiaTriNhap = 0;
            decimal TongGiaTriXuat = 0;
            var DonHangNhaps = !TimKiemLoaiDonHang || LoaiDonHang == "Nhập" ? db.DonHangNhaps.Include(d => d.HangDonHangNhaps).ToList() : null;
            var DonHangXuats = !TimKiemLoaiDonHang || LoaiDonHang == "Xuất" ? db.DonHangXuats.Include(d => d.HangDonHangXuats).ToList() : null;
            if (!TimKiemLoaiDonHang || LoaiDonHang == "Nhập")
            {
                foreach (var DonHangNhap in DonHangNhaps)
                {
                    DoanhThu doanhThu = new DoanhThu();
                    doanhThu.MaDH = DonHangNhap.MaDHN;
                    doanhThu.GiamGia = DonHangNhap.GiamGia;
                    doanhThu.KieuGiamGia = DonHangNhap.KieuGiamGia;
                    doanhThu.NgayThucHien = DonHangNhap.NgayNhap;
                    doanhThu.LoaiDonHang = "Nhập";
                    decimal TongDonHang = 0;
                    foreach (var HangDonhangNhap in DonHangNhap.HangDonHangNhaps)
                    {
                        TongDonHang += HangDonhangNhap.SoLuong * db.Hangs.Where(d => d.MaH == HangDonhangNhap.MaH).First().GiaNhap;
                    }
                    if (DonHangNhap.KieuGiamGia == "VNĐ" && DonHangNhap.GiamGia != null)
                    {
                        TongDonHang -= decimal.Parse(DonHangNhap.GiamGia);
                    }
                    else if (DonHangNhap.KieuGiamGia == "%" && DonHangNhap.GiamGia != null)
                    {
                        TongDonHang -= TongDonHang % 100 * decimal.Parse(DonHangNhap.GiamGia);
                    }
                    if (
                        ((TimKiemGiaTriDonHang && GiaTriDonHangBDDecimal <= TongDonHang && TongDonHang <= GiaTriDonHangKTDecimal) || !TimKiemGiaTriDonHang)
                        &&
                        ((TimKiemNgayThucHien && NgayThucHienBDDate <= DonHangNhap.NgayNhap && DonHangNhap.NgayNhap <= NgayThucHienKTDate) || !TimKiemNgayThucHien)    
                    ) {
                        TongGiaTriNhap += TongDonHang;
                        doanhThu.TongDonHang = TongDonHang;
                        doanhThus.Add(doanhThu);
                    }
                }
            }
            if (!TimKiemLoaiDonHang || LoaiDonHang == "Xuất")
            {
                foreach (var DonHangXuat in DonHangXuats)
                {
                    DoanhThu doanhThu = new DoanhThu();
                    doanhThu.MaDH = DonHangXuat.MaDHX;
                    doanhThu.GiamGia = DonHangXuat.GiamGia;
                    doanhThu.KieuGiamGia = DonHangXuat.KieuGiamGia;
                    doanhThu.LoaiDonHang = "Xuất";
                    doanhThu.NgayThucHien = DonHangXuat.NgayXuat;
                    decimal TongDonHang = 0;
                    foreach (var HangDonhangXuat in DonHangXuat.HangDonHangXuats)
                    {
                        TongDonHang += HangDonhangXuat.SoLuong * db.Hangs.Where(d => d.MaH == HangDonhangXuat.MaH).First().GiaNhap;
                    }
                    if (DonHangXuat.KieuGiamGia == "VNĐ" && DonHangXuat.GiamGia != null)
                    {
                        TongDonHang -= decimal.Parse(DonHangXuat.GiamGia);
                    }
                    else if (DonHangXuat.KieuGiamGia == "%" && DonHangXuat.GiamGia != null)
                    {
                        TongDonHang -= TongDonHang % 100 * decimal.Parse(DonHangXuat.GiamGia);
                    }
                    if (
                        ((TimKiemGiaTriDonHang && GiaTriDonHangBDDecimal <= TongDonHang && TongDonHang <= GiaTriDonHangKTDecimal) || !TimKiemGiaTriDonHang)
                        &&
                        ((TimKiemNgayThucHien && NgayThucHienBDDate <= DonHangXuat.NgayXuat && DonHangXuat.NgayXuat <= NgayThucHienKTDate) || !TimKiemNgayThucHien)    
                    ) {
                        TongGiaTriXuat += TongDonHang;
                        doanhThu.TongDonHang = TongDonHang;
                        doanhThus.Add(doanhThu);
                    }
                }
            }
            ViewBag.TongGiaTriNhap = TongGiaTriNhap;
            ViewBag.TongGiaTriXuat = TongGiaTriXuat;
            return View("Index", doanhThus);
        }
    }
}