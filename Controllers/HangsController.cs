﻿using System;
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
    public class HangsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: Hangs
        public ActionResult Index()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) 
                return RedirectToAction("Index", "Login");
            var hangs = db.Hangs.Where(d => d.DaNhap == true).Include(h => h.MauHang).Include(h => h.NhaCungCap);
            return View(hangs.ToList());
        }

        // GET: Hangs/Details/5
        public ActionResult Details(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hang hang = db.Hangs.Find(id);
            if (hang == null)
            {
                return HttpNotFound();
            }
            return View(hang);
        }

        public ActionResult TimKiem(string MaH, string TenMH, string GiaNhapBD, string GiaNhapKT, string GiaBanBD, string GiaBanKT, string NgayNhapBD, string NgayNhapKT, string HanSuDungBD, string HanSuDungKT, string TenNCC)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            ViewBag.MaH = MaH;
            ViewBag.TenMH = TenMH;
            ViewBag.GiaNhap = GiaNhapBD;
            ViewBag.GiaNhap = GiaNhapKT;
            ViewBag.GiaBan = GiaBanBD;
            ViewBag.GiaBan = GiaBanKT;
            ViewBag.NgayNhapBD = NgayNhapBD;
            ViewBag.NgayNhapKT = NgayNhapKT;
            ViewBag.HanSuDungBD = HanSuDungBD;
            ViewBag.HanSuDungKT = HanSuDungKT;
            ViewBag.TenNCC = TenNCC;

            bool TimKiemGiaNhap = GiaNhapBD != "" && GiaNhapKT != "" ? true : false;
            Decimal GiaNhapBDDecimal = new Decimal();
            Decimal GiaNhapKTDecimal = new Decimal();
            if (TimKiemGiaNhap)
            {
                GiaNhapBDDecimal = decimal.Parse(GiaNhapBD);
                GiaNhapKTDecimal = decimal.Parse(GiaNhapKT);
            }
            bool TimKiemGiaBan = GiaBanBD != "" && GiaBanKT != "" ? true : false;
            Decimal GiaBanBDDecimal = new Decimal();
            Decimal GiaBanKTDecimal = new Decimal();
            if (TimKiemGiaBan)
            {
                GiaBanBDDecimal = decimal.Parse(GiaNhapBD);
                GiaBanKTDecimal = decimal.Parse(GiaNhapKT);
            }
            bool TimKiemNgayNhap = NgayNhapBD != "" && NgayNhapKT != "" ? true : false;
            DateTime NgayNhapBDDate = new DateTime();
            DateTime NgayNhapKTDate = new DateTime();
            if (TimKiemNgayNhap)
            {
                NgayNhapBDDate = DateTime.ParseExact(NgayNhapBD, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                NgayNhapKTDate = DateTime.ParseExact(NgayNhapKT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            bool TimKiemHSD = HanSuDungBD != "" && HanSuDungKT != "" ? true : false;
            DateTime HanSuDungBDDate = new DateTime();
            DateTime HanSuDungKTDate = new DateTime();
            if (TimKiemHSD)
            {
                HanSuDungBDDate = DateTime.ParseExact(HanSuDungBD, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                HanSuDungKTDate = DateTime.ParseExact(HanSuDungKT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            var Hangs = db.Hangs.Where(Hang =>
                (MaH == "" || Hang.MaH.Contains(MaH)) &&
                (TenMH == "" || Hang.MauHang.TenMH.Contains(TenMH)) &&
                ((TimKiemGiaNhap && GiaNhapBDDecimal <= Hang.GiaNhap && Hang.GiaNhap <= GiaNhapKTDecimal) || !TimKiemGiaNhap) &&
                ((TimKiemGiaBan && GiaBanBDDecimal <= Hang.GiaBan && Hang.GiaBan <= GiaBanBDDecimal) || !TimKiemGiaBan) &&
                ((TimKiemNgayNhap && NgayNhapBDDate <= Hang.NgayNhap && Hang.NgayNhap <= NgayNhapKTDate) || !TimKiemNgayNhap) &&
                ((TimKiemHSD && HanSuDungBDDate <= Hang.HanSuDung && Hang.HanSuDung <= HanSuDungKTDate) || !TimKiemHSD) &&
                (TenNCC == "" || Hang.NhaCungCap.TenNCC.Contains(TenNCC)) &&
                Hang.DaNhap == true
            );
            return View("Index", Hangs);
        }

        //public ActionResult SearchHang(string HanSuDung = "", string NgayNhap = "",
        //    string GiaNhap = "", string SoLuong = "", string GiaBan = "")
        //{
        //    string GiaNhapMin = GiaNhap, GiaBanMin = GiaBan;
        //    ViewBag.maNV = HanSuDung;
        //    ViewBag.NgayNhap = NgayNhap;
        //    ViewBag.SoLuong = SoLuong;
        //    if (GiaNhap == "")
        //    {
        //        ViewBag.GiaNhap = "";
        //        GiaNhapMin = "0";
        //    }
        //    else
        //    {
        //        ViewBag.GiaNhapMin = GiaNhap;
        //        GiaNhapMin = GiaNhap;
        //    }
        //    if (GiaBan == "")
        //    {
        //        GiaBanMin = Int32.MaxValue.ToString();
        //        ViewBag.GiaBanMin = "";// Int32.MaxValue.ToString(); 
        //    }
        //    else
        //    {
        //        ViewBag.GiaBanMin = GiaBan;
        //        GiaBanMin = GiaBan;
        //    }
        //    //ViewBag.TenNCC = new SelectList(db.NhaCungCaps, "TenNCC");
        //    //ViewBag.TenMH = new SelectList(db.MauHangs, "TenMH");
        //    var timHangs = db.Hangs.SqlQuery("Hang_TimKiem'" + HanSuDung + "','" + NgayNhap + "','" + GiaNhap + "','" + SoLuong + "','" + GiaBan + "'");
        //    if (timHangs.Count() == 0)
        //        ViewBag.TB = "Không có thông tin tìm kiếm.";
        //    return View(timHangs.ToList());
        //}

        // GET: Hangs/Create
        public ActionResult Create()
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            ViewBag.MaMH = new SelectList(db.MauHangs, "MaMH", "TenMH");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }

        // POST: Hangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaH,MaMH,MaNCC,HanSuDung,NgayNhap,GiaNhap,SoLuong,GiaBan")] Hang hang)
        {
            if (ModelState.IsValid)
            {
                db.Hangs.Add(hang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaMH = new SelectList(db.MauHangs, "MaMH", "TenMH", hang.MaMH);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", hang.MaNCC);
            return View(hang);
        }

        // GET: Hangs/Edit/5
        public ActionResult Edit(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hang hang = db.Hangs.Find(id);
            if (hang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaMH = new SelectList(db.MauHangs, "MaMH", "TenMH", hang.MaMH);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", hang.MaNCC);
            return View(hang);
        }

        // POST: Hangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaH,MaMH,MaNCC,HanSuDung,NgayNhap,GiaNhap,SoLuong,GiaBan")] Hang hang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaMH = new SelectList(db.MauHangs, "MaMH", "TenMH", hang.MaMH);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", hang.MaNCC);
            return View(hang);
        }

        // GET: Hangs/Delete/5
        public ActionResult Delete(string id)
        {
            NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null)
                return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hang hang = db.Hangs.Find(id);
            if (hang == null)
            {
                return HttpNotFound();
            }
            return View(hang);
        }

        // POST: Hangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Hang hang = db.Hangs.Find(id);
            db.Hangs.Remove(hang);
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
