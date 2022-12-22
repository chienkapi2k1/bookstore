using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    [MetadataTypeAttribute(typeof(SachMetadata))]
    public partial class Sach
    {
        internal sealed class SachMetadata
        {
            [Display(Name = "Mã Sách: ")]
            public int MaSach { get; set; }
            [Display(Name = "Tên Sách: ")]
            public string TenSach { get; set; }
            [Display(Name = "Tác Giả: ")]
            public string TacGia { get; set; }
            [Display(Name = "Năm Xuất Bản: ")]
            public Nullable<int> NamXB { get; set; }
            [Display(Name = "Lần Xuất Bản: ")]
            public Nullable<int> LanXB { get; set; }
            [Display(Name = "Tóm Tắt: ")]
            public string TomTat { get; set; }
            [Display(Name = "Tổng Số Trang: ")]
            public string TongSoTrang { get; set; }
            [Display(Name = "Số Lượng: ")]
            public Nullable<int> SoLuong { get; set; }
            [Display(Name = "Tập: ")]
            public Nullable<int> Tap { get; set; }
            [Display(Name = "Tổng Số Tập: ")]
            public Nullable<int> TongSoTap { get; set; }
            [Display(Name = "Giá Trị Sách: ")]
            public Nullable<double> GiaTriSach { get; set; }
            [Display(Name = "Giảm Giá: ")]

            public Nullable<double> GiamGia { get; set; }
            [Display(Name = "Đánh Giá: ")]

            public Nullable<double> DanhGia { get; set; }
            [Display(Name = "Giới Thiệu: ")]
            public string GioiThieu { get; set; }
            [Display(Name = "Trạng Thái: ")]
            public Nullable<int> TrangThai { get; set; }
            [Display(Name = "Ảnh: ")]
            public string Anh { get; set; }
        }      
    }
}