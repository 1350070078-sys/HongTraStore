using System;

namespace HongTraStore.Models
{
    public class KhachHang
    {
        public int Id { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? MatKhau { get; set; }
        public string? SoDienThoai { get; set; }
        public string VaiTro { get; set; } = "KhachHang";
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}