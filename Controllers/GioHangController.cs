using Microsoft.AspNetCore.Mvc;
using HongTraStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HongTraStore.Controllers
{
    public class GioHangController : Controller
    {
        private readonly AppDbContext _db;
        const string KEY = "GioHang";

        public GioHangController(AppDbContext db) { _db = db; }

        private List<GioHangItem> LayGio()
        {
            var json = HttpContext.Session.GetString(KEY);
            if (string.IsNullOrEmpty(json))
                return new List<GioHangItem>();
            return JsonSerializer.Deserialize<List<GioHangItem>>(json)
                   ?? new List<GioHangItem>();
        }

        private void LuuGio(List<GioHangItem> gio)
        {
            HttpContext.Session.SetString(KEY,
                JsonSerializer.Serialize(gio));
        }

        public IActionResult Index()
        {
            var gio = LayGio();
            return View(gio);
        }
        [HttpPost]
        public IActionResult ThemVaoGio(
            int sanPhamId, int soLuong, string? ghiChu)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var sp = _db.SanPhams.Find(sanPhamId);
            if (sp == null)
                return RedirectToAction("Index", "DonHang");

            var gio = LayGio();
            var item = gio.FirstOrDefault(x => x.SanPhamId == sanPhamId);

            if (item != null)
            {
                item.SoLuong += soLuong;
                item.GhiChu = ghiChu;
            }
            else
            {
                gio.Add(new GioHangItem
                {
                    SanPhamId = sp.Id,
                    TenSanPham = sp.TenSanPham,
                    Gia = sp.Gia,
                    SoLuong = soLuong,
                    GhiChu = ghiChu
                });
            }

            LuuGio(gio);
            TempData["Success"] = $"Đã thêm {sp.TenSanPham} vào giỏ!";
            return RedirectToAction("Index", "DonHang");
        }

        [HttpPost]
        public IActionResult XoaKhoi(int sanPhamId)
        {
            var gio = LayGio();
            gio.RemoveAll(x => x.SanPhamId == sanPhamId);
            LuuGio(gio);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatSoLuong(int sanPhamId, int soLuong)
        {
            var gio = LayGio();
            var item = gio.FirstOrDefault(x => x.SanPhamId == sanPhamId);
            if (item != null)
            {
                if (soLuong <= 0)
                    gio.RemoveAll(x => x.SanPhamId == sanPhamId);
                else
                    item.SoLuong = soLuong;
            }
            LuuGio(gio);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DatHangTuGio()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var gio = LayGio();
            if (!gio.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            // Tạo mã đơn hàng chung cho cả lần đặt
            string maDon = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");

            foreach (var item in gio)
            {
                var sp = await _db.SanPhams.FindAsync(item.SanPhamId);
                if (sp == null || sp.SoLuongTon < item.SoLuong)
                {
                    TempData["Error"] = $"{item.TenSanPham} không đủ hàng!";
                    return RedirectToAction("Index");
                }

                var donHang = new DonHang
                {
                    MaDonHang = maDon,
                    KhachHangId = userId.Value,
                    SanPhamId = item.SanPhamId,
                    SoLuong = item.SoLuong,
                    GhiChu = item.GhiChu,
                    TrangThai = "Chờ xử lý",
                    TrangThaiThanhToan = "Chưa thanh toán",
                    SoTienThanhToan = item.ThanhTien,
                    NgayDat = DateTime.Now
                };

                sp.SoLuongTon -= item.SoLuong;
                _db.DonHangs.Add(donHang);
            }

            await _db.SaveChangesAsync();
            HttpContext.Session.Remove("GioHang");
            TempData["Success"] = "Đặt hàng thành công! 🎉";
            return RedirectToAction("LichSu", "DonHang");
        }
    }
}