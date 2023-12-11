using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: GioHang
        public ActionResult Index ()
        {
            return View();
        }
        public ActionResult XoaSPKhoiGioHang (int iMaSach)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                listGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (listGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }


        public ActionResult CapNhatGioHang (int iMaSach, FormCollection f)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult GioHang ()
        {
            List<GioHang> listGioHang = LayGioHang();
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }

        public ActionResult XacNhanDonHang ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatHang ()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }

        [HttpPost]
        public ActionResult DatHang (FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();

            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }





        public ActionResult XoaGioHang ()
        {
            List<GioHang> listGioHang = LayGioHang();
            listGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }

        public ActionResult GioHangPartial ()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }


        public List<GioHang> LayGioHang ()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        public ActionResult ThemGioHang (int ms, string url)
        {
            List<GioHang> lGioHang = LayGioHang();

            GioHang sp = lGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }

        private int TongSoLuong ()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongSoLuong += listGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        private double TongTien ()
        {
            double iTongTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongTien += listGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;

        }

    }
}