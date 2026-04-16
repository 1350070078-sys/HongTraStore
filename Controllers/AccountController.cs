using Microsoft.AspNetCore.Mvc;
using HongTraStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HongTraStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) { _db = db; }

        // Đăng ký - GET
        public IActionResult Register() => View();

        // Đăng ký - POST
        [HttpPost]
        public async Task<IActionResult> Register(KhachHang kh)
        {
            var exists = await _db.KhachHangs
                .AnyAsync(x => x.Email == kh.Email);
            if (exists)
            {
                ViewBag.Error = "Email đã tồn tại!";
                return View();
            }
            kh.VaiTro = "KhachHang";
            kh.NgayTao = DateTime.Now;
            _db.KhachHangs.Add(kh);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("UserId", kh.Id);
            HttpContext.Session.SetString("UserName", kh.HoTen ?? "");
            HttpContext.Session.SetString("VaiTro", kh.VaiTro);
            return RedirectToAction("Index", "Home");
        }
        // Đăng nhập - GET
        public IActionResult Login() => View();

        // Đăng nhập - POST
        [HttpPost]
        public async Task<IActionResult> Login(string email, string matkhau)
        {
            if (email == "admin@hongtra.vn" && matkhau == "admin123")
            {
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("VaiTro", "Admin");
                return RedirectToAction("QuanLy", "DonHang");
            }
            var kh = await _db.KhachHangs
                .FirstOrDefaultAsync(x => x.Email == email
                                       && x.MatKhau == matkhau);
            if (kh == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }
            HttpContext.Session.SetInt32("UserId", kh.Id);
            HttpContext.Session.SetString("UserName", kh.HoTen ?? "");
            HttpContext.Session.SetString("VaiTro", kh.VaiTro);
            return RedirectToAction("Index", "Home");
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Admin - Danh sách khách hàng
        public async Task<IActionResult> DanhSachKhach()
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login");
            var ds = await _db.KhachHangs.ToListAsync();
            return View(ds);
        }

        // Admin - Sửa khách - GET
        public async Task<IActionResult> SuaKhach(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login");
            var kh = await _db.KhachHangs.FindAsync(id);
            return View(kh);
        }

        // Admin - Sửa khách - POST
        [HttpPost]
        public async Task<IActionResult> SuaKhach(KhachHang kh)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login");
            var existing = await _db.KhachHangs.FindAsync(kh.Id);
            if (existing != null)
            {
                existing.HoTen = kh.HoTen;
                existing.Email = kh.Email;
                existing.SoDienThoai = kh.SoDienThoai;
                if (!string.IsNullOrEmpty(kh.MatKhau))
                    existing.MatKhau = kh.MatKhau;
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "Đã cập nhật thông tin khách hàng!";
            return RedirectToAction("DanhSachKhach");
        }
        // Admin - Xóa khách
        public async Task<IActionResult> XoaKhach(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login");
            var kh = await _db.KhachHangs.FindAsync(id);
            if (kh != null) _db.KhachHangs.Remove(kh);
            await _db.SaveChangesAsync();
            return RedirectToAction("DanhSachKhach");
        }
    }
}