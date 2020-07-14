using FreeTime1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeTime1.Controllers
{
    public class HomeController : Controller
    {
        private QLTapHoaEntities db = new QLTapHoaEntities();
        public ActionResult Index()
        {
            // NguoiDung sNguoiDung = Session["nguoiDung"] as NguoiDung;
            // if (sNguoiDung == null || db.NguoiDungs.Where(d => d.MaND == sNguoiDung.MaND).FirstOrDefault() == null) return RedirectToAction("Index", "Login");
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}