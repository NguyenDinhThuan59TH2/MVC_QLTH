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
    public class MauHangsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: MauHangs
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) 
                return RedirectToAction("Index", "Login");
            return View(db.MauHangs.ToList());
        }

        // GET: MauHangs/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauHang mauHang = db.MauHangs.Find(id);
            if (mauHang == null)
            {
                return HttpNotFound();
            }
            return View(mauHang);
        }
        public ActionResult TimKiem(string MaMH, string TenMH, string DonVi, string ChuThich)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            var MauHangs = db.MauHangs.Where(MauHang =>
                (MaMH == "" || MauHang.MaMH.Contains(MaMH)) &&
                (TenMH == "" || MauHang.TenMH.Contains(TenMH)) &&
                (DonVi == "" || MauHang.DonVi.Contains(DonVi)) &&
                (ChuThich == "" || MauHang.ChuThich.Contains(ChuThich))
            );
            ViewBag.MaMH = MaMH;
            ViewBag.TenMH = TenMH;
            ViewBag.DonVi = DonVi;
            ViewBag.ChuThich = ChuThich;
            return View("Index", MauHangs);
        }

        // GET: MauHangs/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            return View();
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

        // POST: MauHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaMH,TenMH,DonVi,Anh,ChuThich")] MauHang mauHang)
        {
            if (ModelState.IsValid)
            {

                var Anh = Request.Files["Anh"];
                mauHang.Anh = "Default.jpg";
                if (Anh != null)
                {
                    if (!CheckFileType(Anh.FileName))
                    {
                        ViewBag.LoiFile = "Kiểu File không được hỗ trợ!";
                        return View(mauHang);
                    }
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/MauHangs/" + FileName);
                    Anh.SaveAs(path);
                    mauHang.Anh = FileName;
                }
                int count = db.MauHangs.Count() + 1;
                mauHang.MaMH = "MH" + count.ToString();
                db.MauHangs.Add(mauHang);
                db.SaveChanges();
                ViewBag.TaoThanhCong = "Thêm mẫu hàng " + mauHang.TenMH + " thành công!";
                return View("Index", db.MauHangs.ToList());
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View(mauHang);
        }

        // GET: MauHangs/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauHang mauHang = db.MauHangs.Find(id);
            if (mauHang == null)
            {
                return HttpNotFound();
            }
            return View(mauHang);
        }

        // POST: MauHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string MaMH, string TenMH, string DonVi, string ChuThich)
        {
            MauHang mauHang= db.MauHangs.SingleOrDefault(n => n.MaMH == MaMH);
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            if (mauHang != null)
            {
                var Anh = Request.Files["Anh"];
                if (Anh.FileName != "")
                {
                    if (!CheckFileType(Anh.FileName))
                    {
                        ViewBag.LoiFile = "Kiểu File không được hỗ trợ!";
                        return View(mauHang);
                    }
                    string FileName = System.IO.Path.GetFileName(Anh.FileName);
                    var path = Server.MapPath("/Images/MauHangs/" + FileName);
                    Anh.SaveAs(path);
                    System.Diagnostics.Debug.WriteLine(FileName);
                    mauHang.Anh = FileName;
                }
                mauHang.TenMH = TenMH;
                mauHang.DonVi = DonVi;
                mauHang.ChuThich = ChuThich;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: MauHangs/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MauHang mauHang = db.MauHangs.Find(id);
            if (mauHang == null)
            {
                return HttpNotFound();
            }
            return View(mauHang);
        }

        // POST: MauHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MauHang mauHang = db.MauHangs.Find(id);
            if (db.Hangs.Where(d => d.MaMH == mauHang.MaMH).FirstOrDefault() != null )
            {
                ViewBag.XoaThatBai = "Xóa mặt hàng " + mauHang.TenMH + " không thành công! Mặt hàng này vẫn đang được sử dụng";
                return View("Index", db.MauHangs.ToList());
            }
            db.MauHangs.Remove(mauHang);
            db.SaveChanges();
            ViewBag.XoaThanhCong = "Xóa mẫu hàng " + mauHang.TenMH + " thành công!";
            return View("Index", db.MauHangs.ToList());
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
