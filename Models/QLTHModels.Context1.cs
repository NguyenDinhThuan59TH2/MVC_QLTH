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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLTapHoaEntities : DbContext
    {
        public QLTapHoaEntities()
            : base("name=QLTapHoaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DonHangNhap> DonHangNhaps { get; set; }
        public virtual DbSet<DonHangXuat> DonHangXuats { get; set; }
        public virtual DbSet<Hang> Hangs { get; set; }
        public virtual DbSet<HangDonHangNhap> HangDonHangNhaps { get; set; }
        public virtual DbSet<HangDonHangXuat> HangDonHangXuats { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<MauHang> MauHangs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<NhomHang> NhomHangs { get; set; }
        public virtual DbSet<TaiKhoanKhachHang> TaiKhoanKhachHangs { get; set; }
    }
}
