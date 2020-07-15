using FreeTime1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeTime1.Controllers
{
    public class HomeController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index(string ThongBao = null)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            // if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            KhachHang sKhachHang = Session["khachHang"] as KhachHang;
            if (sNguoiDung == null || sKhachHang != null)
            {
                if (sKhachHang != null)
                {
                    DonHangDat donHangDat = db.DonHangDats.Where(d => d.MaKH == sKhachHang.MaKH && d.TrangThai == "Đang đặt").FirstOrDefault();
                    if (donHangDat != null)
                    {
                        ViewBag.SoLuongHangDat = db.HangDonHangDats.Where(d => d.MaDHD == donHangDat.MaDHD).Count();
                    }
                }
                if (ThongBao != null)
                {
                    ViewBag.ThongBao = ThongBao;
                }
                return View(db.Hangs.Where(i => i.SoLuong > 0).Include(i => i.MauHang).ToList());
            }
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}