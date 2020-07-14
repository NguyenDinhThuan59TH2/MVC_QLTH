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
    public class HangDonHangXuats1Controller : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: HangDonHangXuats1
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            var hangDonHangXuats = db.HangDonHangXuats.Include(h => h.DonHangXuat).Include(h => h.Hang);
            return View(hangDonHangXuats.ToList());
        }

        // GET: HangDonHangXuats1/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangDonHangXuat hangDonHangXuat = db.HangDonHangXuats.Find(id);
            if (hangDonHangXuat == null)
            {
                return HttpNotFound();
            }
            return View(hangDonHangXuat);
        }

        // GET: HangDonHangXuats1/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            ViewBag.MaDHX = new SelectList(db.DonHangXuats, "MaDHX", "MaKH");
            ViewBag.MaH = new SelectList(db.Hangs, "MaH", "MaMH");
            return View();
        }

        // POST: HangDonHangXuats1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDHX,MaH,SoLuong")] HangDonHangXuat hangDonHangXuat)
        {
            if (ModelState.IsValid)
            {
                db.HangDonHangXuats.Add(hangDonHangXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDHX = new SelectList(db.DonHangXuats, "MaDHX", "MaKH", hangDonHangXuat.MaDHX);
            ViewBag.MaH = new SelectList(db.Hangs, "MaH", "MaMH", hangDonHangXuat.MaH);
            return View(hangDonHangXuat);
        }

        // GET: HangDonHangXuats1/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangDonHangXuat hangDonHangXuat = db.HangDonHangXuats.Find(id);
            if (hangDonHangXuat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDHX = new SelectList(db.DonHangXuats, "MaDHX", "MaKH", hangDonHangXuat.MaDHX);
            ViewBag.MaH = new SelectList(db.Hangs, "MaH", "MaMH", hangDonHangXuat.MaH);
            return View(hangDonHangXuat);
        }

        // POST: HangDonHangXuats1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDHX,MaH,SoLuong")] HangDonHangXuat hangDonHangXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hangDonHangXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDHX = new SelectList(db.DonHangXuats, "MaDHX", "MaKH", hangDonHangXuat.MaDHX);
            ViewBag.MaH = new SelectList(db.Hangs, "MaH", "MaMH", hangDonHangXuat.MaH);
            return View(hangDonHangXuat);
        }

        // GET: HangDonHangXuats1/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangDonHangXuat hangDonHangXuat = db.HangDonHangXuats.Find(id);
            if (hangDonHangXuat == null)
            {
                return HttpNotFound();
            }
            return View(hangDonHangXuat);
        }

        // POST: HangDonHangXuats1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HangDonHangXuat hangDonHangXuat = db.HangDonHangXuats.Find(id);
            db.HangDonHangXuats.Remove(hangDonHangXuat);
            db.SaveChanges();
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
