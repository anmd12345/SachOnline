using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;

namespace SACH_ONLINE.Controllers
{
    public class SachOnlineController : Controller
    {
        dbSachOnlineDataContext data = new dbSachOnlineDataContext();
        private List<SACH> LaySachMoi (int count)
        {
            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        private List<SACH> LaySachBanNhieu (int count)
        {
            return data.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }

        public ActionResult Index ()
        {
            var listSach = LaySachMoi(6);
            return View(listSach);
        }

        public ActionResult SachTheoChuDe (int iMaCD, int? page)
        {
            ViewBag.MaCD = iMaCD;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == iMaCD select s;
            return View(sach.ToPagedList(iPageNum, iSize));
        }

        public ActionResult SachTheoNhaXuatBan (int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }

        public ActionResult ChuDePartial ()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }

        public ActionResult SachBanNhieuPartial ()
        {
            var listSach = LaySachBanNhieu(6);
            return PartialView(listSach);
        }

        public ActionResult ChiTietSach (int id)
        {
            var sach = from s in data.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }

        public ActionResult NavPartial ()
        {
            return PartialView();
        }

        public ActionResult SliderPartial ()
        {
            return PartialView();
        }

        public ActionResult FooterPartial ()
        {
            return PartialView();
        }

        public ActionResult NhaXuatBanPartial ()
        {
            var listNXB = from cd in data.NHAXUATBANs select cd;
            return PartialView(listNXB);
        }

        public ActionResult HeaderPartial ()
        {
            return PartialView();
        }
    }
}