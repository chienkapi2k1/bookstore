using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    [MetadataTypeAttribute(typeof(NguoiDungMetadata))]
    public partial class NguoiDung 
    {
        internal sealed class NguoiDungMetadata
        {
            public int MaNguoiDung { get; set; }
            [StringLength(50)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu")]
            public string HoTen { get; set; }
            public string GioiTinh { get; set; }
            public Nullable<int> Tuoi { get; set; }
            [StringLength(255)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu")]
            [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
            public string Email { get; set; }
            [StringLength(255)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Không phải là số điện thoại hợp lệ")]
            public string SDT { get; set; }
            public Nullable<int> TrangThai { get; set; }
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
            public string Role { get; set; }
            public string DiaChi { get; set; }
            public string GhiChu { get; set; }
        }
    }
}