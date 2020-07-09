namespace FreeTime1.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class NguoiDung
    {
        [Key]
        [Display(Name = "Mã Người Dùng")]
        [StringLength(5)]
        public string MaND { get; set; }
        [Key]
        [Display(Name = "Tài Khoản")]
        [Required(ErrorMessage = "Tài khoản là bắt buộc")]
        public string TaiKhoan { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string MatKhau { get; set; }
        [Display(Name = "Nhập Lại Mật Khẩu")]
        [Compare("MatKhau")]
        [Required(ErrorMessage = "Chưa xác nhận mật khẩu")]
        [NotMapped]
        public string NhapLaiMatKhau { get; set; }
        [Key]
        [Display(Name = "Chức vụ")]
        [Required(ErrorMessage = "Chức vụ là bắt buộc")]
        public string ChucVu { get; set; }
        [Key]
        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Họ tên người dùng là bắt buộc")]
        public string HoTen { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 11 số, xin nhập lại")]
        [Phone(ErrorMessage = "Số điện thoại phải là chữ số, xin nhập lại")]
        public string SDT { get; set; }
        [Display(Name = "Ảnh")]
        public string Anh { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Giới Tính")]
        public bool GioiTinh { get; set; }
    }
}
