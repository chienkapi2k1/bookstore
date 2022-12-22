using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class NguoiDungController : Controller
    {
        
        BookStoreEntities db = new BookStoreEntities();

        #region Đăng Nhập Đăng Ký
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung khachhang,FormCollection f)
        {                

            if (ModelState.IsValid)     // validate thỏa mãn mới đưa vào db
            {
                // check tai khoan ton tai
                List<TaiKhoan> checktaikhoan = new List<TaiKhoan>();        
                var D = db.NguoiDung.Select(s => s.TaiKhoan).Distinct().ToList();
                foreach (var item in D)
                {
                    checktaikhoan.Add(new TaiKhoan(item));      // lay danh sach tai khoan
                }
                foreach (var item in checktaikhoan)
                {
                    if (khachhang.TaiKhoan == item.taikhoan)
                    {
                        ViewBag.thongbaotk = "- Tên đăng nhập đã tồn tại !!!";
                        return View();
                    }
                }                
                string xnmatkhau = f["XacnhanMatKhau"].ToString();
                string matkhau = f["MatKhau"].ToString();
                if (xnmatkhau != matkhau)
                {
                    ViewBag.thongbaomk = "- Xác nhận mật khẩu không khớp !! Vui lòng nhập lại";
                    return View();
                }
                //ViewBag.checktaiKhoan = checktaikhoan;
                khachhang.Role = "KHACHHANG";
                khachhang.TrangThai = 1;
                db.NguoiDung.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("DangNhap","NguoiDung");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string taikhoan = f["TaiKhoan"].ToString();
            string matkhau = f["MatKhau"].ToString();
            NguoiDung kh = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau && n.TrangThai == 1);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công!!!";
                Session["Role"] = kh.Role;
                Session["MaNguoiDung"] = kh.MaNguoiDung;
                Session["TaiKhoan"] = kh.TaiKhoan;
                Session["HoTen"] = kh.HoTen;
                Session["TrangThai"] = kh.TrangThai;
                Session["DiaChi"] = kh.DiaChi;  // để giao hàng
                return RedirectToAction("TrangChu", "Sach");
            }
            ViewBag.DangNhapThatBai = "Đăng nhập thất bại! Vui lòng kiểm tra lại Tài khoản, Mật khẩu của bạn";
            return View();
        }
        #endregion

        public ActionResult DangXuat()
        {
            if (Session["TaiKhoan"] != null)
            {
                Session.Clear();
                Session.RemoveAll();
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        #region Tài Khoản
        public ActionResult ThongTinTaiKhoan() 
        { 
            if (Session["TaiKhoan"] != null)
            {
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        public ActionResult ChiTietTaiKhoan()
        {
            if (Session["TaiKhoan"] != null)
            {
                if (Session["TaiKhoan"] != null)
                {
                    var s = Session["TaiKhoan"].ToString();
                    NguoiDung nguoidung = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == s && n.TrangThai == 1); // V
                     return View(nguoidung);
                }
               
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpGet]
        public ActionResult ChinhSuaTaiKhoan()
        {
            if (Session["TaiKhoan"] != null)
            {
                var s = Session["TaiKhoan"].ToString();
                NguoiDung nguoidung = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == s); // V
                return View(nguoidung);
            }
            return RedirectToAction("TrangChu", "Sach");
        }
        [HttpPost]
        public ActionResult ChinhSuaTaiKhoan(NguoiDung nguoidung_web)
        {
            if (Session["TaiKhoan"] != null)
            {
                var s = Session["TaiKhoan"].ToString();
                NguoiDung nguoidung_db = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == s); // V
                nguoidung_db.HoTen = nguoidung_web.HoTen;
                nguoidung_db.GioiTinh = nguoidung_web.GioiTinh;
                nguoidung_db.Tuoi = nguoidung_web.Tuoi;
                nguoidung_db.Email = nguoidung_web.Email;
                nguoidung_db.SDT = nguoidung_web.SDT;
                nguoidung_db.DiaChi = nguoidung_web.DiaChi;
                Session["HoTen"] = nguoidung_web.HoTen;
                Session["DiaChi"] = nguoidung_web.DiaChi;  // để giao hàng
                db.SaveChanges();
                TempData["OK"] = "Update user successful!!";
                return RedirectToAction("ChiTietTaiKhoan","NguoiDung");
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        public ActionResult DoiMatKhau()
        {
            if (Session["TaiKhoan"] != null)
            {
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost]
        public ActionResult DoiMatKhau(MatKhau matkhau)
        {

            if(Session["TaiKhoan"] != null)
            {
                if (ModelState.IsValid)
                {
                    var a = Session["TaiKhoan"].ToString();
                    NguoiDung nguoidung = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == a); // V

                    if (matkhau.oldPassword == nguoidung.MatKhau)
                    {
                        nguoidung.MatKhau = matkhau.newPassword;
                        db.SaveChanges();
                        return RedirectToAction("ThongTinTaiKhoan", "NguoiDung");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật Khẩu cũ sai");
                        return View("DoiMatKhau");
                    }
                }
                return View("DoiMatKhau");
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpGet]
        public ActionResult DanhSachYeuThich()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("TrangChu", "Sach");
            }
            int maKH = Convert.ToInt32(Session["MaNguoiDung"]);
            List<Sach> lstSach = db.Sach.OrderBy(n => n.MaSach).ToList(); // list sách theo thự tự mới đến cũ
            ViewBag.ListSach = lstSach;
            List<DanhSachYeuThich> ds_yeuthich = db.DanhSachYeuThich.Where(n => n.MaNguoiDung == maKH).OrderBy(n => n.MaSach).ToList();
            List<Sach> laygia = new List<Sach>();
            foreach(var item in ds_yeuthich)
            {
                foreach(var s in lstSach)
                {
                    if(item.MaSach == s.MaSach)
                    {
                        laygia.Add(s);
                    }
                }
            }
            List<double?> giasach = new List<double?>(); // danh sách giá 

            foreach (var item in laygia)
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

            return View(ds_yeuthich);
        }

        [HttpPost]
        public ActionResult ThemVaoDanhSachYeuThich([Bind(Include = "MaDSYeuThich, MaNguoiDung, MaSach, GhiChu, TrangThai")] DanhSachYeuThich dsyt, int MaSach, string Url)
        {
            if (ModelState.IsValid)
            {
                int maKH = Convert.ToInt32(Session["MaNguoiDung"]);
                List<DanhSachYeuThich> ds_yeuthich = db.DanhSachYeuThich.Where(n => n.MaNguoiDung == maKH).OrderBy(n => n.MaSach).ToList();
                int ok = 0; // ok == 0 : chưa tồn tại ; != 0 tồn tại
                foreach(var item in ds_yeuthich) // ktra tồn tại sách trong list yêu thích hay chưa
                {
                    if (item.MaSach == MaSach)
                        ok++;
                }
                if(ok == 0)
                {
                    dsyt.TrangThai = 1;
                    dsyt.MaNguoiDung = maKH;
                    dsyt.MaSach = MaSach;
                    db.DanhSachYeuThich.Add(dsyt);
                    db.SaveChanges();

                }

            }
            return Redirect(Url);
        }

        //[HttpDelete]
        [HttpPost]
        public ActionResult XoaSach_DanhSachYeuThich(int MaSach)
        {
            int maKH = Convert.ToInt32(Session["MaNguoiDung"]);
            DanhSachYeuThich sach = db.DanhSachYeuThich.SingleOrDefault(n => n.MaSach == MaSach && n.MaNguoiDung == maKH);
            //ktra sách có tồn tại trong ds yêu thích hay không

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DanhSachYeuThich.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("DanhSachYeuThich", "NguoiDung");
        }

        #endregion


        public ActionResult LienHe()
        {
            return View();
        }

        #region Admin Quản Lý

        public ActionResult BangDieuKhien()
        {
            if (Session["TaiKhoan"] != null)
            {
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        #region Quản Lý Sách
        public ActionResult QuanLySach(string ten, string tacgia)
        {
            if (Session["TaiKhoan"] != null)
            {
                if(Session["Role"].ToString() == "ADMIN")
                {  
                    List<Sach> lstSach = db.Sach.OrderByDescending(n => n.MaSach).ToList();
                    // láy dữ liệu select option cho ô tìm kiếm
                    List<TacGia> tacGias = new List<TacGia>();
                    var TG = db.Sach.Select(s => s.TacGia).Distinct().ToList();
                    foreach (var item in TG)
                    {
                        tacGias.Add(new TacGia(item));
                    }
                    ViewBag.ListTacGia = tacGias; // ds đưa vào select option
                    //timkiem
                    if ((ten != null && ten != "") || (tacgia != null && tacgia != ""))
                    {
                        ten = ten.Trim();
                        ViewBag.Ten = ten.Trim();
                        tacgia = tacgia.Trim();
                        if (tacgia == "-")
                        {
                            tacgia = "";
                        }
                        TempData["TacGia"] = tacgia.Trim();
                        lstSach = lstSach.Where(n => n.TenSach.Contains(ten) && n.TacGia.Contains(tacgia)).OrderByDescending(n => n.MaSach).ToList();
                    }        
                    return View(lstSach);
                }        
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpGet]
        public ActionResult ThemSach()
        {
            if(Session["Role"] != null)
            {
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSach([Bind(Include = "MaSach, TenSach, TacGia, NamXB, LanXB, TomTat , TongSoTrang, SoLuong, Tap, TongSoTap, GiaTriSach, GiamGia, DanhGia, GioiThieu, TrangThai, Anh")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                sach.TrangThai = 1;
                db.Sach.Add(sach);
                db.SaveChanges();
                return RedirectToAction("ThemSach");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChinhSuaSach(int MaSach)
        {
            if (Session["Role"] != null || MaSach.ToString() != null)
            {
                if (MaSach == null)
                {
                    return HttpNotFound();
                }
                Sach sach = db.Sach.Find(MaSach);
                if (sach == null)
                {
                    return HttpNotFound();
                }
                return View(sach);
            }
            return RedirectToAction("TrangChu", "Sach");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaSach([Bind(Include = "MaSach, TenSach, TacGia, NamXB, LanXB, TomTat , TongSoTrang, SoLuong, Tap, TongSoTap, GiaTriSach, GiamGia, DanhGia, GioiThieu, TrangThai, Anh")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuanLySach", "NguoiDung");
            }
            return RedirectToAction("ChinhSuaSach", "NguoiDung");
        }


        [HttpGet]
        public ActionResult XoaSach(int MaSach)
        {
            if (Session["Role"] != null || MaSach.ToString() != null)
            {
                Sach sach = db.Sach.Single(n => n.MaSach == MaSach);
                if (sach == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sach);
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult XacNhanXoa(int MaSach)
        {
            Sach sach = db.Sach.Single(n => n.MaSach == MaSach);
            var ctTheLoai = from p in db.ChiTietTheLoai
                            where p.MaSach == sach.MaSach
                            select p;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ChiTietTheLoai.RemoveRange(ctTheLoai);
            db.Sach.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("QuanLySach", "NguoiDung");
        }

        #endregion


        #region Quản Lý Chi Tiết Thể Loại

        public ActionResult DSCTTheLoai(string TenSach, string TenTheLoai)
        {
            if (Session["TaiKhoan"] != null)
            {
                if (Session["Role"].ToString() == "ADMIN")
                {
                    List<ChiTietTheLoai> lstCTTL = db.ChiTietTheLoai.OrderByDescending(n => n.MaSach).ToList(); //ds cttheloai show ra
                    List<Sach> lstSach = db.Sach.ToList();
                    List<TheLoai> lstTheLoai = db.TheLoai.ToList();
                    ViewBag.ListSach = lstSach;
                    ViewBag.ListTheLoai = lstTheLoai; //ds đưa vào select option

                    int ms = 0;
                    int mtl = 0;
                    foreach (var item in lstTheLoai)
                    {
                        if (item.TenTheLoai == TenTheLoai)
                        {
                            mtl = item.MaTheLoai;
                        }
                    }
                    foreach (var item in lstSach)
                    {
                        if (item.TenSach == TenSach)
                        {
                            ms = item.MaSach;
                        }
                    }
                    // tìm kiếm
                    if (TenSach != null || TenTheLoai != null)
                    {
                        if (TenSach == "" && TenTheLoai == "")
                        {
                            lstCTTL = lstCTTL.OrderByDescending(n => n.MaSach).ToList();
                        }
                        else
                        {
                            if (TenSach != "" && TenTheLoai != "")
                            {
                                lstCTTL = lstCTTL.Where(n => n.MaSach == ms && n.MaTheLoai == mtl).OrderByDescending(n => n.MaSach).ToList();
                            }
                            else
                            {
                                if (TenTheLoai != "" && TenSach == "")
                                {
                                    lstCTTL = lstCTTL.Where(n => n.MaTheLoai == mtl).OrderByDescending(n => n.MaSach).ToList();
                                }
                                if (TenTheLoai == "" && TenSach != "")
                                {
                                    lstCTTL = lstCTTL.Where(n => n.MaSach == ms).OrderByDescending(n => n.MaSach).ToList();
                                }
                            }
                        }
                        //lstCTTL = lstCTTL.Where(n => (ms == 0 ? n.MaSach.Contains("") : (n.MaSach == ms ) && ((mtl == 0) ? n.MaTheLoai.Contains("") : (n.MaTheLoai == mtl))).OrderByDescending(n => n.MaCTTheLoai).ToList();
                        TempData["TenTheLoai"] = TenTheLoai;
                        TempData["TenSach"] = TenSach;
                    }
                    return View(lstCTTL);
                }
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpGet]
        public ActionResult ThemCTTheLoai()
        {
            if (Session["Role"] != null)
            {
                ViewBag.MaSach = new SelectList(db.Sach.ToList().OrderByDescending(n => n.MaSach), "MaSach", "TenSach");
                ViewBag.MaTheLoai = new SelectList(db.TheLoai.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemCTTheLoai([Bind(Include = "MaChiTietTheLoai, MaTheLoai,  MaSach")] ChiTietTheLoai CtTheLoai)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietTheLoai.Add(CtTheLoai);
                db.SaveChanges();
                return RedirectToAction("DSCTTheLoai", "NguoiDung");
            }

            ViewBag.MaSach = new SelectList(db.Sach.ToList(), "MaSach", "TenSach");
            ViewBag.MaTheLoai = new SelectList(db.TheLoai.ToList(), "MaTheLoai", "TenTheLoai");

            return View();
        }

        [HttpGet]
        public ActionResult ChinhSuaCTTheLoai(int MaCTTheLoai)
        {
            if (Session["Role"] != null || MaCTTheLoai.ToString() != null)
            {
                if (MaCTTheLoai == null)
                {
                    return HttpNotFound();
                }
                ChiTietTheLoai ctTheLoai = db.ChiTietTheLoai.Find(MaCTTheLoai);
                if (ctTheLoai == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MaSach = new SelectList(db.Sach.ToList().OrderByDescending(n => n.MaSach), "MaSach", "TenSach");
                ViewBag.MaTheLoai = new SelectList(db.TheLoai.ToList().OrderBy(n => n.TenTheLoai), "MaTheLoai", "TenTheLoai");

                return View(ctTheLoai);
            }
            return RedirectToAction("TrangChu", "Sach");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaCTTheLoai([Bind(Include = "MaCTTheLoai, MaTheLoai,  MaSach")] ChiTietTheLoai ctTheLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctTheLoai).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DSCTTheLoai", "NguoiDung");
            }
            ViewBag.MaSach = new SelectList(db.Sach.ToList(), "MaSach", "TenSach");
            ViewBag.MaTheLoai = new SelectList(db.TheLoai.ToList(), "MaTheLoai", "TenTheLoai");
            return RedirectToAction("ChinhSuaSach", "NguoiDung");
        }

        [HttpGet]
        public ActionResult XoaCTTheLoai(int MaCTTheLoai)
        {
            if (Session["Role"] != null || MaCTTheLoai.ToString() != null)
            {
                ChiTietTheLoai ctTheLoai = db.ChiTietTheLoai.Single(n => n.MaCTTheLoai == MaCTTheLoai);
                if (ctTheLoai == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ctTheLoai);
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost, ActionName("XoaCTTheLoai")]
        public ActionResult XoaChiTietTheLoai(int MaCTTheLoai)
        {
            ChiTietTheLoai ctTheLoai = db.ChiTietTheLoai.Single(n => n.MaCTTheLoai == MaCTTheLoai);
            if (ctTheLoai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ChiTietTheLoai.Remove(ctTheLoai);
            db.SaveChanges();
            return RedirectToAction("DSCTTheLoai", "NguoiDung");
        }

        #endregion


        #region Quản Lí Thể loại
        public ActionResult DSTheLoai(string TenTheLoai)
        {
            if (Session["TaiKhoan"] != null)
            {
                if (Session["Role"].ToString() == "ADMIN")
                {
                    List<TheLoai> lstTheLoai = db.TheLoai.OrderByDescending(n => n.MaTheLoai).ToList();
                    // láy dữ liệu select option cho ô tìm kiếm

                    //timkiem
                    if (TenTheLoai != null && TenTheLoai != "")
                    {
                        TenTheLoai = TenTheLoai.Trim();
                        ViewBag.TenTheLoai = TenTheLoai.Trim();
                        lstTheLoai = lstTheLoai.Where(n => n.TenTheLoai.Contains(TenTheLoai)).OrderByDescending(n => n.MaTheLoai).ToList();
                    }
                    ViewBag.ListTheLoai = lstTheLoai;
                    return View(lstTheLoai);
                }
            }
            return RedirectToAction("TrangChu", "Sach");
        }


        [HttpGet]
        public ActionResult ThemTheLoai()
        {
            if (Session["Role"] != null)
            {
                return View();
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemTheLoai([Bind(Include = "MaTheLoai, TenTheLoai")] TheLoai theloai)
        {
            if (ModelState.IsValid)
            {
                db.TheLoai.Add(theloai);
                db.SaveChanges();
                return RedirectToAction("DSTheLoai","NguoiDung");
            }
            return View();
        }

        public ActionResult ChinhSuaTheLoai(int MaTheLoai)
        {
            if (Session["Role"] != null || MaTheLoai.ToString() != null)
            {
                if (MaTheLoai == null)
                {
                    return HttpNotFound();
                }
                TheLoai theloai = db.TheLoai.Find(MaTheLoai);
                if (theloai == null)
                {
                    return HttpNotFound();
                }
                return View(theloai);
            }
            return RedirectToAction("TrangChu", "Sach");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaTheLoai([Bind(Include = "MaTheLoai, TenTheLoai")] TheLoai theloai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(theloai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DSTheLoai", "NguoiDung");
            }
            return RedirectToAction("ChinhSuaTheLoai", "NguoiDung");
        }

        [HttpGet]
        public ActionResult XoaTheLoai(int MaTheLoai)
        {
            if (Session["Role"] != null || MaTheLoai.ToString() != null)
            {
                TheLoai theloai = db.TheLoai.Single(n => n.MaTheLoai == MaTheLoai);
                if (theloai == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(theloai);
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        [HttpPost, ActionName("XoaTheLoai")]
        public ActionResult XacNhanXoaTheLoai(int MaTheLoai)
        {
            TheLoai theloai = db.TheLoai.Single(n => n.MaTheLoai == MaTheLoai);
            //var ctTheLoai = from p in db.ChiTietTheLoai
            //                where p.MaSach == sach.MaSach
            //                select p;
            List<ChiTietTheLoai> cttheloai = db.ChiTietTheLoai.Where(n => n.MaTheLoai == MaTheLoai).ToList();
            if (theloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ChiTietTheLoai.RemoveRange(cttheloai);

            db.TheLoai.Remove(theloai);
            db.SaveChanges();
            return RedirectToAction("QuanLySach", "NguoiDung");
        }

        #endregion

        #region Quản lí Đơn Hang

        public ActionResult QuanLyDonHang(string ten, string ngay)
        {
            if (Session["TaiKhoan"] != null)
            {
                if (Session["Role"].ToString() == "ADMIN")
                {
                    List<DonHang> lstDH = db.DonHang.OrderByDescending(n => n.MaDH).ToList();
                    List<NguoiDung> lstKH = db.NguoiDung.OrderByDescending(n => n.MaNguoiDung).ToList();
                    ViewBag.KhachHang = lstKH;
                    //timkiem
                    if ((ten != null && ten != "") || (ngay != null && ngay != ""))
                    {
                        ten = ten.Trim();
                        ViewBag.Ten = ten;
                        ngay = ngay.Trim();
                        ViewBag.Ngay = ngay;

                        List<NguoiDung> makh = db.NguoiDung.Where(n => n.HoTen.Contains(ten)).ToList(); // các khách hàng có tên gần đúng với giá trị ô tìm kiếm
                        List<DonHang> lstDH_TimKiem = new List<DonHang>(); // danh sách chứa kết quả tìm kiếm
                        foreach (var item in makh)
                        {
                            List<DonHang> lstDH_kh = db.DonHang.Where(n => n.MaNguoiDung == item.MaNguoiDung).ToList(); // danh sách đơn hàng có tên các khách hàng ( tìm kiếm khách hàng)
                            foreach (var kh1 in lstDH_kh)
                                lstDH_TimKiem.Add(kh1);    
                        }
                        lstDH_TimKiem = lstDH_TimKiem.Where(n => ((n.NgayTao.Value).ToString()).Contains(ngay)).OrderByDescending(n => n.MaNguoiDung & n.MaDH).ToList(); // tìm kiếm theo ngày
                        return View(lstDH_TimKiem);
                        //lstDH = lstDH.Where(n => n.MaDH == makh && n.NgayTao.ToString().Contains(ngay)).OrderByDescending(n => n.MaNguoiDung & n.MaDH).ToList();
                    }
                    return View(lstDH);
                }
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        public ActionResult DSCTDonHang(string TenSach, int? MaDH)
        {
            if (Session["TaiKhoan"] != null)
            {
                if (Session["Role"].ToString() == "ADMIN")
                {
                    List<ChiTietDonHang> lstCTTL = db.ChiTietDonHang.OrderByDescending(n => n.MaDH).ToList(); //ds cttheloai show ra
                    List<Sach> lstSach = db.Sach.ToList();
                    List<DonHang> lstDonHang = db.DonHang.ToList();
                    ViewBag.ListSach = lstSach;
                    ViewBag.ListDonHang = lstDonHang; //ds đưa vào select option

                    int ms = 0;
                    int mdh = 0;
                    foreach (var item in lstDonHang)
                    {
                        if (item.MaDH == MaDH)
                        {
                            mdh = item.MaDH;
                        }
                    }
                    foreach (var item in lstSach)
                    {
                        if (item.TenSach == TenSach)
                        {
                            ms = item.MaSach;
                        }
                    }
                    //tìm kiếm
                    if (TenSach != null || MaDH != null)
                    {
                        if (TenSach == "" && MaDH == null)
                        {
                            lstCTTL = lstCTTL.OrderByDescending(n => n.MaSach).ToList();
                        }
                        else
                        {
                            if (TenSach != "" && MaDH != null)
                            {
                                lstCTTL = lstCTTL.Where(n => n.MaSach == ms && n.MaDH == mdh).OrderByDescending(n => n.MaSach).ToList();
                            }
                            else
                            {
                                if (MaDH != null && TenSach == "")
                                {
                                    lstCTTL = lstCTTL.Where(n => n.MaDH == mdh).OrderByDescending(n => n.MaSach).ToList();
                                }
                                if (MaDH == null && TenSach != "")
                                {
                                    lstCTTL = lstCTTL.Where(n => n.MaSach == ms).OrderByDescending(n => n.MaSach).ToList();
                                }
                            }
                        }
                        //lstCTTL = lstCTTL.Where(n => (ms == 0 ? n.MaSach.Contains("") : (n.MaSach == ms ) && ((mtl == 0) ? n.MaTheLoai.Contains("") : (n.MaTheLoai == mtl))).OrderByDescending(n => n.MaCTTheLoai).ToList();
                        TempData["MaDonHang"] = MaDH;
                        TempData["TenSach"] = TenSach;
                    }
                    return View(lstCTTL);
                }
            }
            return RedirectToAction("TrangChu", "Sach");
        }

        #endregion 


        #endregion


    }
}