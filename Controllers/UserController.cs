using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class UserController : Controller
    {
        private dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: User
        public ActionResult Index ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy (FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã tồn tại trên hệ thống";
            }
            else
            {
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DienThoai = sDienThoai;
                kh.DiaChi = sDiaChi;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy();
        }


        [HttpGet]
        public ActionResult DangNhap ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap (FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa điền tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Bạn phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công!";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                    return RedirectToAction("DangNhap", "User");
                }
            }
            return View();
        }
    }
}