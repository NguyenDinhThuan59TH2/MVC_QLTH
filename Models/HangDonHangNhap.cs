//------------------------------------------------------------------------------
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
    
    public partial class HangDonHangNhap
    {
        public string MaDHN { get; set; }
        public string MaH { get; set; }
        public int SoLuong { get; set; }
    
        public virtual Hang Hang { get; set; }
        public virtual DonHangNhap DonHangNhap { get; set; }
    }
}
