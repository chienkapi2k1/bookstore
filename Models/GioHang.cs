using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BookStore.Models
{
    public class GioHang
    {
        BookStoreEntities db = new BookStoreEntities();        
        public GioHang(int MaSach)
        {
            this.MaSach = MaSach;
            Sach sanpham = db.Sach.SingleOrDefault(n => n.MaSach == MaSach);
            this.TenSach = sanpham.TenSach;
            this.TacGia = sanpham.TacGia;
            this.Anh = sanpham.Anh;
            this.GiaTriSach = double.Parse(sanpham.GiaTriSach.ToString());
            this.SoLuong = 1;
        }
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string Anh { get; set; }
        public double GiaTriSach { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get { return GiaTriSach * SoLuong; }
        }

    }
}