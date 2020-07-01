using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeTime1.Models
{
    public class DoanhThu
    {
        public string MaDH { get; set; }
        public decimal TongDonHang { get; set; }
        public string LoaiDonHang { get; set; }
        public System.DateTime NgayThucHien { get; set; }
        public string GiamGia { get; set; }
        public string KieuGiamGia { get; set; }

    }
}