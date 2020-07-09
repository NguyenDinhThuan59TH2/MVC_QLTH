﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FreeTime1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DonHangNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHangNhap()
        {
            this.HangDonHangNhaps = new HashSet<HangDonHangNhap>();
        }

        [Key]
        [Display(Name = "Mã Đơn Hàng Nhập")]
        public string MaDHN { get; set; }
        [Display(Name = "Mã Nhà Cung Cấp")]
        public string MaNCC { get; set; }
        [Display(Name = "Kiểu Giảm Giá")]
        public string KieuGiamGia { get; set; }
        [Display(Name = "Giảm Giá")]
        public string GiamGia { get; set; }
        [Display(Name = "Ngày Nhập")]
        public System.DateTime NgayNhap { get; set; }
        public decimal TongDonHang { get; set; }    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HangDonHangNhap> HangDonHangNhaps { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
