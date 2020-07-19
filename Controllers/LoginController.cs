using FreeTime1.Models;
using System;
using System.Linq;
using System.Net;
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
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            NguoiDung nguoiDung = db.NguoiDungs.Where(i => i.TaiKhoan == TaiKhoan).FirstOrDefault();
            if (nguoiDung != null)
            {
                if (SecurePasswordHasher.Verify(MatKhau, nguoiDung.MatKhau))
                {
                    Session["nguoiDung"] = nguoiDung;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TaiKhoanKhachHang taiKhoanKhachHang = db.TaiKhoanKhachHangs.Where(i => i.TaiKhoan == TaiKhoan).FirstOrDefault();
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
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        private bool CheckFileType(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }
        public string Insert(TaiKhoanKhachHang entity)
        {
            db.TaiKhoanKhachHangs.Add(entity);
            db.SaveChanges();
            return entity.MaKH;
        }
        public bool CheckTaiKhoan(string taiKhoan)
        {
            return db.TaiKhoanKhachHangs.Count(x => x.TaiKhoan == taiKhoan) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.KhachHangs.Count(x => x.Email == email) > 0;
        }
        public bool CheckSDT(string SDT)
        {
            return db.KhachHangs.Count(x => x.SDT == SDT) > 0;
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if(CheckTaiKhoan(model.TaiKhoan))
                {
                    ModelState.AddModelError("AddModelError", "Tài khoản đã tồn tại");
                }
                else if(CheckEmail(model.Email))
                {
                    ModelState.AddModelError("AddModelError", "Email đã tồn tại");
                }
                else if (CheckSDT(model.SDT))
                {
                    ModelState.AddModelError("AddModelError", "Số điện thoại đã được sử dụng");
                }
                else
                {
                    KhachHang khachHang = new KhachHang();
                    int count = db.KhachHangs.Count() + 1;
                    khachHang.MaKH = "KH" + count.ToString();
                    khachHang.HoTen = model.HoTen;
                    khachHang.Anh = "MacDinh.png";
                    khachHang.DiaChi = "";
                    khachHang.SDT = model.SDT;
                    khachHang.Email = model.Email;
                    khachHang.NgaySinh = model.NgaySinh;
                    khachHang.GioiTinh = true;
                    khachHang.DaXoa = false;

                    TaiKhoanKhachHang taiKhoanKhachHang = new TaiKhoanKhachHang();
                    taiKhoanKhachHang.MaKH = khachHang.MaKH;
                    taiKhoanKhachHang.TaiKhoan = model.TaiKhoan;
                    taiKhoanKhachHang.MatKhau = SecurePasswordHasher.Hash(model.MatKhau);
                    
                    db.KhachHangs.Add(khachHang);
                    db.TaiKhoanKhachHangs.Add(taiKhoanKhachHang);
                    db.SaveChanges();
                    
                    ViewBag.Success = "Đăng ký thành công";
                    ViewBag.TaiKhoan = model.TaiKhoan;
                    ViewBag.MatKhau = model.MatKhau;
                    return View("Index");
                }    
            }
            return View(model);
        }
    }
}