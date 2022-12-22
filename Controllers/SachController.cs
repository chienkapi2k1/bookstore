using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class SachController : Controller
    {
        BookStoreEntities db = new BookStoreEntities();
        // GET: Sach
        public ActionResult TrangChu()
        {
            List<Sach> lstSach = db.Sach.Where(n => n.TrangThai != 0 && n.GiamGia > 0).OrderByDescending(n => n.GiamGia).ToList(); // list sách theo thự tự mới đến cũ
            List<double?> giasach = new List<double?>(); // danh sách giá sách sau khi tính giảm giá

            foreach (var item in lstSach)
            {
                if (item.GiamGia != 0)
                {
                    double? gia = item.GiaTriSach - item.GiaTriSach * item.GiamGia / 100;
                    giasach.Add(gia);
                }
                else
                {
                    giasach.Add(item.GiaTriSach);
                }
            }
            ViewBag.GiaSach = giasach;

            // top 8 sách mới nhất
            List<Sach> lstSachMoi = db.Sach.Where(n => n.TrangThai != 0).OrderByDescending(n => n.MaSach).Take(8).ToList(); // list sách theo thự tự mới đến cũ
            ViewBag.lstSachMoi = lstSachMoi;

            List<double?> giasachmoi = new List<double?>(); // danh sách giá 

            foreach (var item in lstSachMoi)
            {
                if (item.GiamGia != 0)
                {
                    double? gia = item.GiaTriSach - item.GiaTriSach * item.GiamGia / 100;
                    giasachmoi.Add(gia);
                }
                else
                {
                    giasachmoi.Add(item.GiaTriSach);
                }
            }
            ViewBag.GiaSachMoi = giasachmoi;
            //
            if (lstSach.Count == 0 || lstSachMoi.Count == 0)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(lstSach);
        }
        public PartialViewResult TheLoaiPartial()
        {
            return PartialView(db.TheLoai.ToList());
        }

        public ActionResult DanhSach()
        {
            List<Sach> lstSach = db.Sach.Where(n => n.TrangThai != 0).OrderByDescending(n => n.MaSach).ToList(); // list sách theo thự tự mới đến cũ
            List<double?> giasach = new List<double?>(); // danh sách giá 

            foreach(var item in lstSach)
            {
                if(item.GiamGia != 0)
                {
                    double? gia = item.GiaTriSach - item.GiaTriSach * item.GiamGia / 100;
                    giasach.Add(gia);
                }
                else
                {
                    giasach.Add(item.GiaTriSach);
                }
            }
            ViewBag.GiaSach = giasach;
            if (lstSach.Count == 0)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.lstSach = db.Sach.ToList();
            return View(lstSach);
        }

        public ViewResult SanPhamTheoLoai(int MaTheLoai)
        {
            TheLoai lsp = db.TheLoai.SingleOrDefault(n => n.MaTheLoai == MaTheLoai);

            if (lsp == null)
            {   
                Response.StatusCode = 404;
                return null;
            }
            List<ChiTietTheLoai> lstCTTheLoai = db.ChiTietTheLoai.Where(n => n.MaTheLoai == MaTheLoai).OrderByDescending(n => n.MaSach).ToList();

            List<Sach> lstSach = new List<Sach>();
            foreach(var item in lstCTTheLoai)
            {
                Sach s = db.Sach.SingleOrDefault(n => n.MaSach == item.MaSach && n.TrangThai != 0);
                if (s != null)
                {
                    lstSach.Add(s);
                }
            };

            List<double?> giasach = new List<double?>(); // danh sách giá 

            foreach (var item in lstSach)
            {
                if (item.GiamGia != 0)
                {
                    double? gia = item.GiaTriSach - item.GiaTriSach * item.GiamGia / 100;
                    giasach.Add(gia);
                }
                else
                {
                    giasach.Add(item.GiaTriSach);
                }
            }
            ViewBag.GiaSach = giasach;
            if (lstSach.Count == 0)
            {
                ViewBag.lstSanPham = "Không có sách nào thuộc thể loại này";
            }
            //ViewBag.MaTheLoai = MaTheLoai;
            ViewBag.TenTheLoai = db.TheLoai.SingleOrDefault(n => n.MaTheLoai == MaTheLoai).TenTheLoai;
            //ViewBag.lstSanPham = db.TheLoai.ToList();
            return View(lstSach);
        }

        public ActionResult ChiTietSach(int MaSach)
        {
            Sach sach = db.Sach.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            double? giasach = sach.GiaTriSach; 
            if (sach.GiamGia != 0)
            {
                giasach = sach.GiaTriSach - sach.GiaTriSach * sach.GiamGia / 100;
            }
            ViewBag.GiaSach = giasach;
            return View(sach);
        }

    }
}