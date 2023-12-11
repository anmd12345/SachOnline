using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private dbSachOnlineDataContext db = new dbSachOnlineDataContext();


        // GET: Admin/Home
        public ActionResult Index ()
        {
            return View();
        }

        public ActionResult LoginLogout ()
        {
            return PartialView("LoginLogoutPartial");
        }

        public ActionResult GioHangPartial ()
        {
            return PartialView("GioHangPartial");
        }


        [HttpGet]
        public ActionResult Login ()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login (FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau
            == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Ten đang nhập hoặc mật khẩu khong đung";
                return View();
            }
        }
    }
}
