using Microsoft.AspNetCore.Mvc;
using HongTraStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HongTraStore.Controllers
{
    public class DonHangController : Controller
    {
        private readonly AppDbContext _db;
        public DonHangController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var sanPhams = await _db.SanPhams
                .Where(s => s.SoLuongTon > 0)
                .ToListAsync();
            return View(sanPhams);
        }

        [HttpPost]
        public async Task<IActionResult> DatHang(
            int sanPhamId, int soLuong, string? ghiChu)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var sp = await _db.SanPhams.FindAsync(sanPhamId);
            if (sp == null || sp.SoLuongTon < soLuong)
            {
                TempData["Error"] = "Không đủ hàng tồn kho!";
                return RedirectToAction("Index");
            }

            string maDon = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var donHang = new DonHang
            {
                MaDonHang = maDon,
                KhachHangId = userId.Value,
                SanPhamId = sanPhamId,
                SoLuong = soLuong,
                GhiChu = ghiChu,
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chưa thanh toán",
                SoTienThanhToan = (sp.Gia * soLuong),
                NgayDat = DateTime.Now
            };
            sp.SoLuongTon -= soLuong;
            _db.DonHangs.Add(donHang);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đặt hàng thành công! 🎉";
            return RedirectToAction("LichSu");
        }

        public async Task<IActionResult> LichSu()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var donHangs = await _db.DonHangs
                .Include(d => d.SanPham)
                .Include(d => d.KhachHang)
                .Where(d => d.KhachHangId == userId.Value)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }

        public async Task<IActionResult> QuanLy()
        {
            var vaiTro = HttpContext.Session.GetString("VaiTro");
            if (vaiTro != "Admin")
                return RedirectToAction("Login", "Account");

            var donHangs = await _db.DonHangs
                .Include(d => d.SanPham)
                .Include(d => d.KhachHang)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(
            int id, string trangThai, string thanhToan)
        {
            var vaiTro = HttpContext.Session.GetString("VaiTro");
            if (vaiTro != "Admin")
                return RedirectToAction("Login", "Account");

            var don = await _db.DonHangs.FindAsync(id);
            if (don != null)
            {
                // Cập nhật tất cả đơn cùng mã đơn hàng
                var nhom = await _db.DonHangs
                    .Where(d => d.MaDonHang == don.MaDonHang)
                    .ToListAsync();

                foreach (var d in nhom)
                {
                    d.TrangThai = trangThai;
                    d.TrangThaiThanhToan = thanhToan;

                    // Ghi nhận thời gian thanh toán
                    if (thanhToan == "Đã thanh toán"
                        && d.ThoiGianThanhToan == null)
                    {
                        d.ThoiGianThanhToan = DateTime.Now;
                    }
                    // Nếu hủy thanh toán thì xóa thời gian
                    if (thanhToan == "Chưa thanh toán")
                    {
                        d.ThoiGianThanhToan = null;
                    }
                }
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("QuanLy");
        }

        [HttpPost]
        public async Task<IActionResult> HuyDon(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var don = await _db.DonHangs.FindAsync(id);
            if (don != null && don.KhachHangId == userId.Value
                && don.TrangThai == "Chờ xử lý")
            {
                don.TrangThai = "Đã hủy";
                var sp = await _db.SanPhams.FindAsync(don.SanPhamId);
                if (sp != null) sp.SoLuongTon += don.SoLuong;
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đã hủy đơn hàng!";
            }
            return RedirectToAction("LichSu");
        }

        [HttpPost]
        public async Task<IActionResult> HuyDonNhom(string maDonHang)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var danhSach = await _db.DonHangs
                .Include(d => d.SanPham)
                .Where(d => d.MaDonHang == maDonHang
                         && d.KhachHangId == userId.Value
                         && d.TrangThai == "Chờ xử lý")
                .ToListAsync();

            foreach (var don in danhSach)
            {
                don.TrangThai = "Đã hủy";
                if (don.SanPham != null)
                    don.SanPham.SoLuongTon += don.SoLuong;
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Đã hủy đơn hàng!";
            return RedirectToAction("LichSu");
        }
    }
}