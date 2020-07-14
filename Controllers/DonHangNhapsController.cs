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
    public class DonHangNhapsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: DonHangNhaps
        public ActionResult Index()
        {
            var donHangNhaps = db.DonHangNhaps.Include(d => d.NhaCungCap);
            foreach (DonHangNhap donHangNhap in donHangNhaps)
            {
                donHangNhap.TongDonHang = 0;
                var hangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN);
                foreach (HangDonHangNhap hangDonHangNhap in hangDonHangNhaps)
                {
                    Hang hang = db.Hangs.Where(d => d.MaH == hangDonHangNhap.MaH).First();
                    donHangNhap.TongDonHang += Math.Round(hangDonHangNhap.SoLuong * hang.GiaNhap, 0);

                }
                // tinh giam gia
                if (donHangNhap.KieuGiamGia != "" && donHangNhap.GiamGia != null)
                {
                    if (donHangNhap.KieuGiamGia == "VNĐ")
                    {
                        donHangNhap.TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
                    }
                    else if (donHangNhap.KieuGiamGia == "%")
                    {
                        donHangNhap.TongDonHang -= donHangNhap.TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);
                    }
                }
            }
            return View(donHangNhaps);
        }
        // GET: DonHangNhaps/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hangDonHangNhaps = db.HangDonHangNhaps.Where(hangDonHangNhap => hangDonHangNhap.MaDHN == id).Include(hangDonHangNhap => hangDonHangNhap.DonHangNhap);
            decimal TongDonhang = 0;
            foreach (var hangDonHangNhap in hangDonHangNhaps)
            {
                var NhaCungCap = db.NhaCungCaps.Where(d => d.MaNCC == hangDonHangNhap.DonHangNhap.MaNCC).FirstOrDefault();
                var Hang = db.Hangs.Where(d => d.MaH == hangDonHangNhap.MaH).FirstOrDefault();
                var MauHang = db.MauHangs.Where(d => d.MaMH == Hang.MaMH).FirstOrDefault();
                hangDonHangNhap.DonHangNhap.NhaCungCap = NhaCungCap;
                hangDonHangNhap.Hang = Hang;
                hangDonHangNhap.Hang.MauHang = MauHang;
                TongDonhang += hangDonHangNhap.SoLuong * hangDonHangNhap.Hang.GiaNhap;
            }
            if (hangDonHangNhaps == null)
            {
                return HttpNotFound();
            }
            DonHangNhap donHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == id).Include(d => d.NhaCungCap).FirstOrDefault();
            if (donHangNhap.KieuGiamGia != "" && donHangNhap.GiamGia != null)
            {
                if (donHangNhap.KieuGiamGia == "VNĐ")
                {
                    TongDonhang -= decimal.Parse(donHangNhap.GiamGia);
                } else if (donHangNhap.KieuGiamGia == "%") {
                    TongDonhang -= TongDonhang / 100 * decimal.Parse(donHangNhap.GiamGia);
                }
            }
            ViewBag.TongDonhang = String.Format("{0:n0}", TongDonhang) + "VNĐ";
            ViewBag.MaDHN = donHangNhap.MaDHN;
            ViewBag.TenNCC = donHangNhap.NhaCungCap.TenNCC;
            ViewBag.NgayNhap = donHangNhap.NgayNhap;
            ViewBag.GiamGia = donHangNhap.GiamGia;
            ViewBag.KieuGiamGia = donHangNhap.KieuGiamGia;
            return View(hangDonHangNhaps);
        }

        // GET: DonHangNhaps/Create
        public ActionResult Create()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }
        public ActionResult AddStock (string MaDHN)
        {
            var DonHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == MaDHN).FirstOrDefault();
            return View("Create", DonHangNhap);
        }

        public ActionResult TimKiem(string MaDHN, string TenNCC, string NgayNhapBD, string NgayNhapKT, string KieuGiamGia)
        {
            bool TimKiemNgayNhap = NgayNhapBD != "" && NgayNhapKT != "" ? true : false;
            DateTime NgayNhapBDDate = new DateTime();
            DateTime NgayNhapKTDate = new DateTime();
            if (TimKiemNgayNhap)
            {
                NgayNhapBDDate = DateTime.ParseExact(NgayNhapBD, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                NgayNhapKTDate = DateTime.ParseExact(NgayNhapKT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            var DonHangNhaps = db.DonHangNhaps.Include(DonHangNhap => DonHangNhap.NhaCungCap).Where(DonHangNhap =>
                (MaDHN == "" || DonHangNhap.MaDHN.Contains(MaDHN)) &&
                (TenNCC == "" || DonHangNhap.NhaCungCap.TenNCC.Contains(TenNCC)) &&
                (KieuGiamGia == null || DonHangNhap.KieuGiamGia.Contains(KieuGiamGia)) &&
                ((TimKiemNgayNhap && NgayNhapBDDate <= DonHangNhap.NgayNhap && DonHangNhap.NgayNhap <= NgayNhapKTDate) || !TimKiemNgayNhap)
            );
            foreach (DonHangNhap donHangNhap in DonHangNhaps)
            {
                donHangNhap.TongDonHang = 0;
                var hangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN);
                foreach (HangDonHangNhap hangDonHangNhap in hangDonHangNhaps)
                {
                    Hang hang = db.Hangs.Where(d => d.MaH == hangDonHangNhap.MaH).First();
                    donHangNhap.TongDonHang += hangDonHangNhap.SoLuong * hang.GiaNhap;
                }
                // tinh giam gia
                if (donHangNhap.KieuGiamGia != "" && donHangNhap.GiamGia != null)
                {
                    if (donHangNhap.KieuGiamGia == "VNĐ")
                    {
                        donHangNhap.TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
                    }
                    else if (donHangNhap.KieuGiamGia == "%")
                    {
                        donHangNhap.TongDonHang -= donHangNhap.TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);
                    }
                }
            }
            ViewBag.MaDHN = MaDHN;
            ViewBag.TenNCC = TenNCC;
            ViewBag.NgayNhapBD = NgayNhapBD;
            ViewBag.NgayNhapKT = NgayNhapKT;
            ViewBag.KieuGiamGia = KieuGiamGia;
            return View("Index", DonHangNhaps);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocument([Bind(Include = "MaNCC,NgayNhap,GiamGia,KieuGiamGia")] DonHangNhap donHangNhap)
        {
            if (ModelState.IsValid)
            {
                int count = db.DonHangNhaps.Count() + 1;
                donHangNhap.MaDHN = "DHN" + count.ToString();
                if (donHangNhap.KieuGiamGia == "True")
                {
                    donHangNhap.KieuGiamGia = "VNĐ";
                } else
                {
                    donHangNhap.KieuGiamGia = "%";
                }
                db.DonHangNhaps.Add(donHangNhap);
                db.SaveChanges();
                donHangNhap.NhaCungCap = db.NhaCungCaps.Where(d => d.MaNCC == donHangNhap.MaNCC).FirstOrDefault();
                donHangNhap.HangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN).ToList();
                decimal TongDonHang = 0;
                foreach (var hangDonHangNhap in donHangNhap.HangDonHangNhaps)
                {
                    hangDonHangNhap.Hang = db.Hangs.Where(d => d.MaH == hangDonHangNhap.MaH).FirstOrDefault();
                    hangDonHangNhap.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == hangDonHangNhap.Hang.MaMH).FirstOrDefault();
                    TongDonHang = hangDonHangNhap.SoLuong * hangDonHangNhap.Hang.GiaNhap;
                }
                if (donHangNhap.KieuGiamGia == "%" && donHangNhap.GiamGia != null)
                {
                    TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);
                }
                else if (donHangNhap.KieuGiamGia == "VNĐ" && donHangNhap.GiamGia != null)
                {
                    TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
                }
                ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
                ViewBag.MauHangs = db.MauHangs.ToList();
                ViewBag.Hangs = db.Hangs.Include(d => d.MauHang).ToList();
                return View("Create", donHangNhap);
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View("Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewStock (string MaMH, string SoLuong, string MaDHN, string GiaNhap, string GiaBan, string HanSuDung)
        {
            var donHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == MaDHN).FirstOrDefault();
            int countHang = db.Hangs.Count() + 1;
            Hang hang = new Hang();
            hang.MaH= "H" + countHang.ToString();
            bool loi = false;
            if (SoLuong == "")
            {
                ViewBag.Loi = "Chưa nhập số lượng";
                loi = true;
            }
            else if (int.Parse(SoLuong) <= 0) {
                ViewBag.Loi = "Số lượng phải lớn hơn 0";
                loi = true;
            } else {
                hang.SoLuong = int.Parse(SoLuong);
            }
            if (GiaNhap == "")
            {
                ViewBag.Loi = "Chưa nhập giá nhập";
                loi = true;
            } else if (int.Parse(GiaNhap) <= 0) {
                ViewBag.Loi = "Giá nhập phải lớn hơn 0";
                loi = true;
            } else
            {
                hang.GiaNhap = int.Parse(GiaNhap);
            }
            if (GiaBan == "") {
                ViewBag.Loi = "Chưa nhập giá bán";
                loi = true;
            }
            else if (int.Parse(GiaBan) <= 0)
            {
                ViewBag.Loi = "Giá bán phải lớn hơn 0";
                loi = true;
            } else {
                hang.GiaBan = int.Parse(GiaBan);
            }
            if (HanSuDung == "") {
                ViewBag.Loi = "Chưa nhập hạn sử dụng";
                loi = true;
            } else {
                hang.HanSuDung = DateTime.ParseExact(HanSuDung, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            if (!loi)
            {
                hang.MaMH = MaMH;
                hang.MaNCC = donHangNhap.MaNCC;
                hang.NgayNhap = donHangNhap.NgayNhap;
                System.Diagnostics.Debug.WriteLine("HanSuDung: ", HanSuDung);
                HangDonHangNhap hangDonHangNhap = new HangDonHangNhap();
                hangDonHangNhap.SoLuong = int.Parse(SoLuong);
                hangDonHangNhap.MaH = hang.MaH;
                hangDonHangNhap.MaDHN = donHangNhap.MaDHN;
                db.Hangs.Add(hang);
                db.HangDonHangNhaps.Add(hangDonHangNhap);
                db.SaveChanges();
            }
            donHangNhap.NhaCungCap = db.NhaCungCaps.Where(d => d.MaNCC == donHangNhap.MaNCC).FirstOrDefault();
            donHangNhap.HangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN).ToList();
            decimal TongDonHang = 0;
            foreach (var HangDonHangNhap in donHangNhap.HangDonHangNhaps)
            {
                HangDonHangNhap.Hang = db.Hangs.Where(d => d.MaH == HangDonHangNhap.MaH).FirstOrDefault();
                HangDonHangNhap.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == HangDonHangNhap.Hang.MaMH).FirstOrDefault();
                TongDonHang = HangDonHangNhap.SoLuong * HangDonHangNhap.Hang.GiaNhap;
            }
            if (donHangNhap.KieuGiamGia == "%" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);   
            } else if (donHangNhap.KieuGiamGia == "VNĐ" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
            }
            ViewBag.TongDonHang = TongDonHang;
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Include(d => d.MauHang).ToList();
            return View("Create", donHangNhap);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInStock(string MaDHN, string MaH, string SoLuong)
        {
            var donHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == MaDHN).FirstOrDefault();
            Hang hang = db.Hangs.Single(d => d.MaH == MaH);
            var HDHN = db.HangDonHangNhaps.Where(d => d.MaDHN == MaDHN && d.MaH == MaH).FirstOrDefault();
            bool loi = false;
            if (SoLuong == "")
            {
                ViewBag.Loi = "Chưa nhập số lượng!";
                loi = true;
            } else if (int.Parse(SoLuong) <= 0)
            {
                ViewBag.Loi = "Số lượng phải lớn hơn 0!";
                loi = true;
            }
            if (!loi)
            {
                if (HDHN != null)
                {
                    HDHN.SoLuong += int.Parse(SoLuong);
                } else {
                    HangDonHangNhap hangDonHangNhap = new HangDonHangNhap();
                    hangDonHangNhap.SoLuong = int.Parse(SoLuong);
                    hangDonHangNhap.MaH = MaH;
                    hangDonHangNhap.MaDHN = donHangNhap.MaDHN;
                    db.HangDonHangNhaps.Add(hangDonHangNhap);
                }
                hang.SoLuong += int.Parse(SoLuong);
                db.SaveChanges();

            }
            donHangNhap.NhaCungCap = db.NhaCungCaps.Where(d => d.MaNCC == donHangNhap.MaNCC).FirstOrDefault();
            donHangNhap.HangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN).ToList();
            decimal TongDonHang = 0;
            foreach (var HangDonHangNhap in donHangNhap.HangDonHangNhaps)
            {
                HangDonHangNhap.Hang = db.Hangs.Where(d => d.MaH == HangDonHangNhap.MaH).FirstOrDefault();
                HangDonHangNhap.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == HangDonHangNhap.Hang.MaMH).FirstOrDefault();
                TongDonHang = HangDonHangNhap.SoLuong * HangDonHangNhap.Hang.GiaNhap;
            }
            if (donHangNhap.KieuGiamGia == "%" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);
            }
            else if (donHangNhap.KieuGiamGia == "VNĐ" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
            }
            ViewBag.TongDonHang = TongDonHang;
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Include(d => d.MauHang).ToList();
            return View("Create", donHangNhap);
        }

        public ActionResult CreateMauHangs()
        {
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
        public ActionResult CreateMauHangs([Bind(Include = "MaMH,TenMH,DonVi,Anh,ChuThich")] MauHang mauHang)
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
            return View("Create", donHangNhap);
        }

        public ActionResult DeleteInStock(string MaDHN, string MaH)
        {
            DonHangNhap donHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == MaDHN).FirstOrDefault();
            Hang hang = db.Hangs.Single(d => d.MaH == MaH);
            if (hang != null)
            {
                var HDHN = db.HangDonHangNhaps.Where(d => d.MaDHN == MaDHN && d.MaH == MaH).FirstOrDefault();
                if(HDHN != null)
                {
                    db.HangDonHangNhaps.Remove(HDHN);
                    hang.SoLuong -= HDHN.SoLuong;
                    db.SaveChanges();
                }
            }
            donHangNhap.NhaCungCap = db.NhaCungCaps.Where(d => d.MaNCC == donHangNhap.MaNCC).FirstOrDefault();
            donHangNhap.HangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == donHangNhap.MaDHN).ToList();
            decimal TongDonHang = 0;
            foreach (var HangDonHangNhap in donHangNhap.HangDonHangNhaps)
            {
                HangDonHangNhap.Hang = db.Hangs.Where(d => d.MaH == HangDonHangNhap.MaH).FirstOrDefault();
                HangDonHangNhap.Hang.MauHang = db.MauHangs.Where(d => d.MaMH == HangDonHangNhap.Hang.MaMH).FirstOrDefault();
                TongDonHang = HangDonHangNhap.SoLuong * HangDonHangNhap.Hang.GiaNhap;
            }
            if (donHangNhap.KieuGiamGia == "%" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= TongDonHang / 100 * decimal.Parse(donHangNhap.GiamGia);
            }
            else if (donHangNhap.KieuGiamGia == "VNĐ" && donHangNhap.GiamGia != null)
            {
                TongDonHang -= decimal.Parse(donHangNhap.GiamGia);
            }
            ViewBag.TongDonHang = String.Format("{0:n0}", TongDonHang);
            ViewBag.MauHangs = db.MauHangs.ToList();
            ViewBag.Hangs = db.Hangs.Include(d => d.MauHang).ToList();
            return View("Create", donHangNhap);
        }
        public ActionResult DeleteDocument(string MaDHN)
        {
            DonHangNhap donHangNhap = db.DonHangNhaps.Where(d => d.MaDHN == MaDHN).FirstOrDefault();
            if (donHangNhap != null)
            {
                var HangDonHangNhaps = db.HangDonHangNhaps.Where(d => d.MaDHN == MaDHN).Include(d => d.Hang).ToList();
                foreach (HangDonHangNhap hangDonHangNhap in HangDonHangNhaps)
                {
                    hangDonHangNhap.Hang.SoLuong -= hangDonHangNhap.SoLuong;
                    db.HangDonHangNhaps.Remove(hangDonHangNhap);
                }
                db.DonHangNhaps.Remove(donHangNhap);
                db.SaveChanges();
                ViewBag.XoaThanhCong = "Xóa đơn hàng " + MaDHN + " thành công";
            }
            var donHangNhaps = db.DonHangNhaps.Include(d => d.NhaCungCap);
            return View("Index", donHangNhaps.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDHN,MaNCC,NgayNhap,GiamGia,KieuGiamGia")] DonHangNhap donHangNhap)
        {
            if (ModelState.IsValid)
            {
                db.DonHangNhaps.Add(donHangNhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", donHangNhap.MaNCC);
            return View(donHangNhap);
        }

        // GET: DonHangNhaps/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangNhap donHangNhap = db.DonHangNhaps.Find(id);
            if (donHangNhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", donHangNhap.MaNCC);
            return View(donHangNhap);
        }

        // POST: DonHangNhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDHN,MaNCC,NgayNhap,GiamGia,KieuGiamGia")] DonHangNhap donHangNhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHangNhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", donHangNhap.MaNCC);
            return View(donHangNhap);
        }

        // GET: DonHangNhaps/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHangNhap donHangNhap = db.DonHangNhaps.Find(id);
            if (donHangNhap == null)
            {
                return HttpNotFound();
            }
            return View(donHangNhap);
        }

        // POST: DonHangNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonHangNhap donHangNhap = db.DonHangNhaps.Find(id);
            db.DonHangNhaps.Remove(donHangNhap);
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
