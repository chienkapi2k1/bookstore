using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        BookStoreEntities db = new BookStoreEntities();

        #region Giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                RedirectToAction("TrangChu", "Sach");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>; // khi đã có session giỏ hàng thì != null
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;     // tạo session giỏ hàng khi chưa có lstGH
            }
            return lstGioHang;
        }

        private int TongSoLuong()
        {
            int iTongSL = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSL = lstGioHang.Sum(n => n.SoLuong);
            }
            return iTongSL;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }


        public ActionResult ThemSach_GioHang(int MaSach, string Url)
        {
            Sach sach = db.Sach.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang Sach_Trong_GH = lstGioHang.Find(n => n.MaSach == MaSach);
            
            if (Sach_Trong_GH == null)
            {
                Sach_Trong_GH = new GioHang(MaSach);
                lstGioHang.Add(Sach_Trong_GH);
                ViewBag.TongSoLuong = TongSoLuong();
                return Redirect(Url);
            }
            else
            {
                Sach_Trong_GH.SoLuong++;
                ViewBag.TongSoLuong = TongSoLuong();
                return Redirect(Url);
            }

        }

        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() != 0)
            {
                ViewBag.TongSoLuong = TongSoLuong();
            }
            return PartialView();
        }


        public ActionResult CapNhatGioHang(List<int> listMaSach, List<int> listSL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            for (int i = 0; i < listMaSach.Count; i++)
            {
                GioHang sach = lstGioHang.SingleOrDefault(n => n.MaSach == listMaSach[i]);
                sach.SoLuong = listSL[i];
            }
            //kiểm tra sản phẩm có trong giỏ hàng không
            return Json("200");
        }

        public ActionResult XoaSach_GioHang(int MaSach)
        {
            Sach sach = db.Sach.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            //kiểm tra sản phẩm có trong giỏ hàng không
            GioHang sach_trong_gh = lstGioHang.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach_trong_gh != null)
            {
                lstGioHang.RemoveAll(n => n.MaSach == MaSach);
            }
            if (lstGioHang.Count == 0)
            {
                RedirectToAction("TrangChu", "Sach");
            }
            return RedirectToAction("GioHang","GioHang");
        }
#endregion

        //Đặt hàng
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["TaiKhoan"] != null && Session["DiaChi"] == null)
            {
                return RedirectToAction("ChinhSuaTaiKhoan", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("TrangChu", "Sach");
            }
            List<GioHang> lstGioHang = LayGioHang();
            if(lstGioHang.Count() == 0)
            {
                return RedirectToAction("TrangChu", "Sach");
            }                
            //lưu thông tin vào bảng hóa đơn
            DonHang donhang = new DonHang();
            donhang.NgayTao = DateTime.Now;
            donhang.MaNguoiDung = (int)Session["MaNguoiDung"];
            donhang.TongTien = (int)TongTien();
            db.DonHang.Add(donhang);
            db.SaveChanges();
            // lưu thông tin vào chi tiết hóa đơn
            
            foreach (var item in lstGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDH = donhang.MaDH;
                ctdh.MaSach = item.MaSach;
                ctdh.SoLuong = item.SoLuong;
                ctdh.GiaBan = (int)item.GiaTriSach;
                ctdh.TongTien = (int)item.ThanhTien;
                db.ChiTietDonHang.Add(ctdh);
            }
            if(Session["GioHang"] != null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            db.SaveChanges();
            return RedirectToAction("TrangChu", "Sach");
        }

    }
}