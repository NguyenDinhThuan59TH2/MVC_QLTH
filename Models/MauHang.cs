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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    
    public partial class MauHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MauHang()
        {
            this.Hangs = new HashSet<Hang>();
            this.NhomHangs = new HashSet<NhomHang>();
        }
        [Display(Name = "Mã mẫu hàng")]
        public string MaMH { get; set; }
        [Display(Name = "Tên mẫu hàng")]
        [Required(ErrorMessage = "Tên mẫu hàng là bắt buộc")]
        public string TenMH { get; set; }
        [Display(Name = "Đơn vị")]
        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        public string DonVi { get; set; }
        [Display(Name = "Ảnh")]
        public string Anh { get; set; }
        [Display(Name = "Chú thích")]
        [Required(ErrorMessage = "Chú thích là bắt buộc")]
        public string ChuThich { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hang> Hangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhomHang> NhomHangs { get; set; }
    }
}
