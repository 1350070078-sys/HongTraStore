using System;

namespace HongTraStore.Models
{
    public class DonHang
    {
        public int Id { get; set; }
        public string MaDonHang { get; set; } = "";
        public int KhachHangId { get; set; }
        public KhachHang? KhachHang { get; set; }
        public int SanPhamId { get; set; }
        public SanPham? SanPham { get; set; }
        public int SoLuong { get; set; }
        public string? GhiChu { get; set; }
        public string TrangThai { get; set; } = "Chờ xử lý";
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";
        public string? PhuongThucThanhToan { get; set; }
        public decimal SoTienThanhToan { get; set; } = 0;
        public DateTime? ThoiGianThanhToan { get; set; }
        public DateTime NgayDat { get; set; } = DateTime.Now;
    }
}