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

    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            this.DonHangNhaps = new HashSet<DonHangNhap>();
            this.Hangs = new HashSet<Hang>();
        }
        [Display(Name = "Mã nhà cung cấp")]
        [Required(ErrorMessage = "Mã nhà cung cấp là bắt buộc")]
        public string MaNCC { get; set; }
        [Display(Name = "Tên nhà cung cấp")]
        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
        public string TenNCC { get; set; }
        [Display(Name = "Quốc gia")]
        [Required(ErrorMessage = "Quốc gia là bắt buộc")]
        public string QuoGia { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 11 số, xin nhập lại")]
        [Phone(ErrorMessage = "Số điện thoại phải là chữ số, xin nhập lại")]
        public string SDT { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Xin nhập đúng Email")]
        public string Email { get; set; }
        public bool DaXoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHangNhap> DonHangNhaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hang> Hangs { get; set; }
    }
}
