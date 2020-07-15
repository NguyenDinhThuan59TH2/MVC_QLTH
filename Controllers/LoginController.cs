using FreeTime1.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace FreeTime1.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class LoginController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string TaiKhoan, string MatKhau) {
            NguoiDung nguoiDung = db.NguoiDungs.Where(i => i.TaiKhoan == TaiKhoan).FirstOrDefault();
            if (nguoiDung != null) {
                if (SecurePasswordHasher.Verify(MatKhau, nguoiDung.MatKhau)) {
                    Session["nguoiDung"] = nguoiDung;
                    return RedirectToAction("Index", "Home");
                }
            } else
            {
                TaiKhoanKhachHang taiKhoanKhachHang= db.TaiKhoanKhachHangs.Where(i => i.TaiKhoan == TaiKhoan).FirstOrDefault();
                if (taiKhoanKhachHang != null)
                {
                    if (SecurePasswordHasher.Verify(MatKhau, taiKhoanKhachHang.MatKhau))
                    {
                        KhachHang khachHang = db.KhachHangs.Where(i => i.MaKH == taiKhoanKhachHang.MaKH && i.DaXoa == false).FirstOrDefault();
                        if (khachHang != null)
                        {
                            Session["khachHang"] = khachHang;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View("Index");
        }
        public ActionResult Logout ()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}