namespace HongTraStore.Models
{
    public class GioHangItem
    {
        public int SanPhamId { get; set; }
        public string? TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string? GhiChu { get; set; }
        public decimal ThanhTien => Gia * SoLuong;
    }
}