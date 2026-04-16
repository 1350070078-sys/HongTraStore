namespace HongTraStore.Models
{
    public class SanPham
    {
        public int Id { get; set; }
        public string? TenSanPham { get; set; }
        public string? DanhMuc { get; set; }
        public decimal Gia { get; set; }
        public int SoLuongTon { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
    }
}