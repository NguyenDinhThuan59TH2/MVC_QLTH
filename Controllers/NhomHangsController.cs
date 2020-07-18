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
    public class NhomHangsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: NhomHangs
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            if (TempData["TaoThanhCong"] != null)
            {
                ViewBag.TaoThanhCong = TempData["TaoThanhCong"].ToString();
            }
            if (TempData["XoaThanhCong"] != null)
            {
                ViewBag.XoaThanhCong = TempData["XoaThanhCong"].ToString();
            }
            return View(db.NhomHangs.Where(d => d.DaXoa == false).ToList());
        }

        // GET: NhomHangs/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomHang nhomHang = db.NhomHangs.Find(id);
            if (nhomHang == null)
            {
                return HttpNotFound();
            }
            return View(nhomHang);
        }

        public ActionResult TimKiem(string MaNH, string TenNH)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            var nhomHangs = db.NhomHangs.Where(d =>
                (MaNH == "" || d.MaNH.Contains(MaNH)) &&
                (TenNH == "" || d.TenNH.Contains(TenNH)) &&
                d.DaXoa == false
            );
            ViewBag.MaNH = MaNH;
            ViewBag.TaiKhoan = TenNH;
            return View("Index", nhomHangs);
        }
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        // POST: NhomHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenNH")] NhomHang nhomHang)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            if (ModelState.IsValid)
            {
                if (db.NhomHangs.Where(d => d.TenNH == nhomHang.TenNH).FirstOrDefault() != null)
                {
                    TempData["TaoThanhCong"] = "Tên nhóm hàng đã tồn tại";
                    return RedirectToAction("Index");
                }
                int count = db.NhomHangs.Count() + 1;
                nhomHang.MaNH = "NH" + count.ToString();
                nhomHang.DaXoa = false;
                db.NhomHangs.Add(nhomHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhomHang);
        }

        // GET: NhomHangs/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomHang nhomHang = db.NhomHangs.Find(id);
            if (nhomHang == null)
            {
                return HttpNotFound();
            }
            return View(nhomHang);
        }

        // POST: NhomHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string MaNH, string TenNH)
        {
            NhomHang nhomHang = db.NhomHangs.SingleOrDefault(n => n.MaNH == MaNH && n.DaXoa == false);
            System.Diagnostics.Debug.WriteLine("MaNH: ", MaNH);
            System.Diagnostics.Debug.WriteLine("TenNH: ", TenNH);
            if (db.NhomHangs.Where(d => d.TenNH == TenNH).FirstOrDefault() != null)
            {
                TempData["TaoThanhCong"] = "Tên nhóm hàng đã tồn tại";
                return RedirectToAction("Index");
            }
            if (nhomHang != null)
            {
                nhomHang.TenNH = TenNH;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View();
        }

        // GET: NhomHangs/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomHang nhomHang = db.NhomHangs.Find(id);
            if (nhomHang == null)
            {
                return HttpNotFound();
            }
            return View(nhomHang);
        }

        // POST: NhomHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            NhomHang nhomHang = db.NhomHangs.Find(id);
            nhomHang.DaXoa = true;
            db.SaveChanges();
            TempData["XoaThanhCong"] = "Xóa nhóm hàng " + nhomHang.TenNH + " thành công!";
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
