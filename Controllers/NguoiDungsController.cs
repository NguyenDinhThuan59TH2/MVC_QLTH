using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using FreeTime1.Models;
using Microsoft.Ajax.Utilities;

namespace FreeTime1.Controllers
{
    public class NguoiDungsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: NguoiDungs
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            return View(db.NguoiDungs.ToList());
        }

        // GET: NguoiDungs/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        public ActionResult TimKiem (string MaND, string TaiKhoan, string ChucVu, string HoTen, string SDT, string DiaChi, string GioiTinh)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            bool TimKiemGioiTinh = false;
            if (GioiTinh != null)
            {
                TimKiemGioiTinh = true;
            }
            bool GioiTinhBool = TimKiemGioiTinh ? (GioiTinh == "Nam" ? true : false) : false;
            var NguoiDungs = db.NguoiDungs.Where(NguoiDung =>
                (MaND == "" || NguoiDung.MaND.Contains(MaND)) &&
                (TaiKhoan == "" || NguoiDung.TaiKhoan.Contains(TaiKhoan)) &&
                (ChucVu == "" || NguoiDung.ChucVu.Contains(ChucVu)) &&
                (HoTen == "" || NguoiDung.HoTen.Contains(HoTen)) &&
                (SDT == "" || NguoiDung.SDT.Contains(SDT)) &&
                (DiaChi == "" || NguoiDung.DiaChi.Contains(DiaChi)) &&
                (TimKiemGioiTinh ? NguoiDung.GioiTinh == GioiTinhBool : true)
            );
            ViewBag.MaND = MaND;
            ViewBag.TaiKhoan = TaiKhoan;
            ViewBag.ChucVu = ChucVu;
            ViewBag.HoTen = HoTen;
            ViewBag.SDT= SDT;
            ViewBag.DiaChi = DiaChi;
            ViewBag.GioiTinh = GioiTinh;
            return View("Index", NguoiDungs);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaiKhoan,MatKhau,NhapLaiMatKhau,ChucVu,HoTen,SDT,DiaChi,GioiTinh")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {

                var Anh = Request.Files["Anh"];
                nguoiDung.Anh = "MacDinh.png";
                nguoiDung.DiaChi = nguoiDung.DiaChi == "" ? nguoiDung.DiaChi : "";
                if (Anh.FileName != "")
                {
                    if (!CheckFileType(Anh.FileName))
                    {
                        ViewBag.LoiFile = "Kiểu File không được hỗ trợ!";
                        return View(nguoiDung);
                    }
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/NguoiDungs/" + FileName);
                    Anh.SaveAs(path);
                    nguoiDung.Anh = FileName;
                }
                bool checkTenTK = db.NguoiDungs.Any(d => d.TaiKhoan == nguoiDung.TaiKhoan);
                bool checkSDT = db.NguoiDungs.Any(d => d.SDT == nguoiDung.SDT);
                if (checkTenTK)
                {
                    ViewBag.DaTonTai = "Tên người dùng đã được sử dụng";
                    return View("Create",nguoiDung);
                }
                if (checkSDT)
                {
                    ViewBag.DaTonTaiSDT = "Số điện thoại đã được sử dụng";
                    return View("Create", nguoiDung);
                }
                nguoiDung.MatKhau = SecurePasswordHasher.Hash(nguoiDung.MatKhau);
                nguoiDung.NhapLaiMatKhau = nguoiDung.MatKhau;
                int count = db.NguoiDungs.Count() + 1;
                if (nguoiDung.ChucVu == "Quản lý") nguoiDung.MaND = "QL" + count.ToString();
                else if (nguoiDung.ChucVu == "Nhân viên") nguoiDung.MaND = "NV" + count.ToString();
                else return View(nguoiDung);
                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();
                ViewBag.TaoThanhCong = "Tạo người dùng " + nguoiDung.HoTen + " thành công";
                return View("Index", db.NguoiDungs.ToList());
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View(nguoiDung);
        }
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }
        [HttpPost]
        public ActionResult Edit(string MaND, string HoTen, string SDT, string DiaChi)
        {
            NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(n => n.MaND == MaND);
            if (nguoiDung != null)
            {
                var Anh = Request.Files["Anh"];
                if (Anh.FileName != "")
                {
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/NguoiDungs/" + FileName);
                    Anh.SaveAs(path);
                    System.Diagnostics.Debug.WriteLine(FileName);
                    nguoiDung.Anh = FileName;
                }
                nguoiDung.HoTen = HoTen;
                nguoiDung.SDT = SDT;
                nguoiDung.DiaChi = DiaChi;
                nguoiDung.NhapLaiMatKhau = nguoiDung.MatKhau;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View();
        }
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            db.NguoiDungs.Remove(nguoiDung);
            db.SaveChanges();
            ViewBag.XoaThanhCong = "Xóa người dùng " + nguoiDung.HoTen + " thành công";
            return View("Index", db.NguoiDungs.ToList());
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
