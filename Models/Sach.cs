//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sach()
        {
            this.ChiTietDonHang = new HashSet<ChiTietDonHang>();
            this.ChiTietTheLoai = new HashSet<ChiTietTheLoai>();
            this.DanhGiaSach = new HashSet<DanhGiaSach>();
            this.DanhSachYeuThich = new HashSet<DanhSachYeuThich>();
        }
    
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public Nullable<int> NamXB { get; set; }
        public Nullable<int> LanXB { get; set; }
        public string TomTat { get; set; }
        public string TongSoTrang { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<int> Tap { get; set; }
        public Nullable<int> TongSoTap { get; set; }
        public Nullable<double> GiaTriSach { get; set; }
        public Nullable<double> GiamGia { get; set; }
        public Nullable<double> DanhGia { get; set; }
        public string GioiThieu { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string Anh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTheLoai> ChiTietTheLoai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGiaSach> DanhGiaSach { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhSachYeuThich> DanhSachYeuThich { get; set; }
    }
}
