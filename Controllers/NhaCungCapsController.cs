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
    public class NhaCungCapsController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();

        // GET: NhaCungCaps
        public ActionResult Index()
        {
            return View(db.NhaCungCaps.ToList());
        }

        // GET: NhaCungCaps/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult TimKiem(string TenNCC, string MaNCC, string QuoGia, string DiaChi, string SDT, string Email)
        {
            var NhaCungCaps = db.NhaCungCaps.Where(NhaCungCap =>
                (TenNCC == "" || NhaCungCap.TenNCC.Contains(TenNCC)) &&
                (MaNCC == "" || NhaCungCap.MaNCC.Contains(MaNCC)) &&
                (QuoGia == "" || NhaCungCap.QuoGia.Contains(QuoGia)) &&
                (DiaChi == "" || NhaCungCap.DiaChi.Contains(DiaChi)) &&
                (SDT == "" || NhaCungCap.SDT.Contains(SDT)) &&
                (Email == "" || NhaCungCap.Email.Contains(Email))
            );
            ViewBag.TenNCC = TenNCC;
            ViewBag.MaNCC = MaNCC;
            ViewBag.QuoGia = QuoGia;
            ViewBag.SDT = SDT;
            ViewBag.DiaChi = DiaChi;
            ViewBag.Email = Email;
            return View("Index", NhaCungCaps);
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNCC,TenNCC,QuoGia,DiaChi,SDT,Email")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                int count = db.NhaCungCaps.Count() + 1;
                nhaCungCap.MaNCC = "NCC" + count.ToString();
                db.NhaCungCaps.Add(nhaCungCap);
                db.SaveChanges();
                ViewBag.TaoThanhCong = "Tạo nhà cung cấp " + nhaCungCap.TenNCC + " thành công";
                return View("Index", db.NhaCungCaps.ToList());
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string MaNCC, string TenNCC, string SDT, string DiaChi, string Email, string QuoGia)
        {
            NhaCungCap nhaCungCap = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == MaNCC);
            if (nhaCungCap != null)
            {
                nhaCungCap.TenNCC = TenNCC;
                nhaCungCap.SDT = SDT;
                nhaCungCap.DiaChi = DiaChi;
                nhaCungCap.Email = Email;
                nhaCungCap.QuoGia = QuoGia;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x.ErrorMessage + "\n"));
            return View();
        }

        // GET: NhaCungCaps/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (db.DonHangNhaps.Where(d => d.MaNCC == nhaCungCap.MaNCC).Count() > 0)
            {
                ViewBag.XoaThatBai = "Xóa nhà cung cấp " + nhaCungCap.TenNCC + " không thành công! Nhà cung cấp này đã từng mua hàng";
                return View("Index", db.NhaCungCaps.ToList());
            }
            db.NhaCungCaps.Remove(nhaCungCap);
            db.SaveChanges();
            ViewBag.XoaThanhCong = "Xóa nhà cung cấp " + nhaCungCap.TenNCC + " thành công";
            return View("Index", db.NhaCungCaps.ToList());
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
