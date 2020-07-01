using FreeTime1.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FreeTime1.Controllers
{
    public class LoginController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string TaiKhoan, string MatKhau) {
            NguoiDung nguoiDung = db.NguoiDungs.Where(i => i.TaiKhoan == TaiKhoan).First();
            System.Diagnostics.Debug.WriteLine(nguoiDung);
            if (nguoiDung != null) {
                System.Diagnostics.Debug.WriteLine("co nguoi dung");
                if (SecurePasswordHasher.Verify(MatKhau, nguoiDung.MatKhau)) {
                    ViewBag.TenNhanVien = nguoiDung.HoTen;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Index");
        }
    }
}