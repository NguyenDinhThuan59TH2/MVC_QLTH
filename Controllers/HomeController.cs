using FreeTime1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
                ViewBag.NhomHangs = db.NhomHangs.ToList();
                return View(db.Hangs.Where(i => i.SoLuong > 0).Include(i => i.MauHang).ToList());
            }
            return View();
        }
        public ActionResult SearchGroups(FormCollection formCollection)
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
                List<string> MaNHS = new List<string>(formCollection.AllKeys);
                var mauHangs = db.MauHangs.Include(d => d.NhomHangs).ToList();
                List<string> MaMHS = new List<string>();
                foreach(MauHang mauHang in mauHangs)
                {
                    bool flag = false;
                    foreach(NhomHang nhomHang in mauHang.NhomHangs)
                    {
                        if (MaNHS.Contains(nhomHang.MaNH))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        MaMHS.Add(mauHang.MaMH);
                    }
                }
                var nhomHangs = db.NhomHangs.ToList();
                var viewBagMaNHS = new string[nhomHangs.Count()];
                int i = 0;
                foreach (NhomHang nhom in nhomHangs)
                {
                    if (MaNHS.Contains(nhom.MaNH))
                    {
                        viewBagMaNHS[i] = "checked";
                    }
                    i++;
                }
                ViewBag.NhomHangs = nhomHangs;
                ViewBag.MaNHS = viewBagMaNHS;
                return View("Index", db.Hangs.Where(d => MaMHS.Contains(d.MaMH) && d.SoLuong > 0).ToList());
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