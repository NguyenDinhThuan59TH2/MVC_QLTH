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
    public class KhachHangsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: KhachHangs
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            return View(db.KhachHangs.ToList());
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            return View();
        }

        public ActionResult TimKiem(string MaKH, string Email, string GioiTinh, string HoTen, string SDT, string DiaChi)
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
            var KhachHangs = db.KhachHangs.Where(KhachHang =>
                (MaKH == "" || KhachHang.MaKH.Contains(MaKH)) &&
                (Email == "" || KhachHang.Email.Contains(Email)) &&
                (HoTen == "" || KhachHang.HoTen.Contains(HoTen)) &&
                (SDT == "" || KhachHang.SDT.Contains(SDT)) &&
                (DiaChi == "" || KhachHang.DiaChi.Contains(DiaChi)) &&
                (TimKiemGioiTinh ? KhachHang.GioiTinh == GioiTinhBool : true)
            );
            ViewBag.MaND = MaKH;
            ViewBag.TaiKhoan = Email;
            ViewBag.HoTen = HoTen;
            ViewBag.SDT = SDT;
            ViewBag.DiaChi = DiaChi;
            ViewBag.GioiTinh = GioiTinh;
            return View("Index", KhachHangs);
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

        // POST: KhachHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTen,Anh,DiaChi,SDT,Email,NgaySinh,GioiTinh")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {

                var Anh = Request.Files["Anh"];
                khachHang.Anh = "MacDinh.png";
                khachHang.DiaChi = khachHang.DiaChi == "" ? khachHang.DiaChi : "";
                if (Anh != null)
                {
                    if (!CheckFileType(Anh.FileName))
                    {
                        ViewBag.LoiFile = "Kiểu File không được hỗ trợ!";
                        return View(khachHang);
                    }
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/KhachHangs/" + FileName);
                    Anh.SaveAs(path);
                    khachHang.Anh = FileName;
                }
                bool checkEmail = db.KhachHangs.Any(d => d.Email == khachHang.Email);
                bool checkSDT = db.KhachHangs.Any(d => d.SDT == khachHang.SDT);
                if (checkEmail)
                {
                    ViewBag.DaTonTai = "Email đã được sử dụnng";
                    return View("Create",khachHang);
                }
                if (checkSDT)
                {
                    ViewBag.DaTonTai2 = "Số điện thoại đã được sử dụng";
                    return View("Create", khachHang);
                }

                int count = db.KhachHangs.Count() + 1;
                khachHang.MaKH = "KH" + count.ToString();
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                ViewBag.TaoThanhCong = "Thêm khách hàng " + khachHang.HoTen + " thành công!";
                return View("Index", db.KhachHangs.ToList());
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string MaKH, string HoTen, string SDT, string DiaChi, string Email)
        {
            KhachHang khachHang = db.KhachHangs.SingleOrDefault(n => n.MaKH == MaKH);
            if (khachHang != null)
            {
                var Anh = Request.Files["Anh"];
                if (Anh.FileName != "")
                {
                    if (!CheckFileType(Anh.FileName))
                    {
                        ViewBag.LoiFile = "Kiểu File không được hỗ trợ!";
                        return View(khachHang);
                    }
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/KhachHangs/" + FileName);
                    Anh.SaveAs(path);
                    //System.Diagnostics.Debug.WriteLine(FileName);
                    khachHang.Anh = FileName;
                }
                khachHang.HoTen = HoTen;
                khachHang.SDT = SDT;
                khachHang.DiaChi = DiaChi;
                khachHang.Email = Email;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View();
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            // var khachHangx = db.KhachHangs.Where(d => d.MaKH == id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (db.DonHangXuats.Where(d => d.MaKH == khachHang.MaKH).Count() > 0)
            {
                ViewBag.XoaThatBai = "Xóa khách hàng " + khachHang.HoTen + " không thành công! Người này đã từng mua hàng";
                return View("Index",db.KhachHangs.ToList());
            }
            db.KhachHangs.Remove(khachHang);
            db.SaveChanges();
            ViewBag.XoaThanhCong = "Xóa khách hàng " + khachHang.HoTen + " thành công!";
            return View("Index", db.KhachHangs.ToList());
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
