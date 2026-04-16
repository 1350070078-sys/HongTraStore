using Microsoft.AspNetCore.Mvc;
using HongTraStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HongTraStore.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly AppDbContext _db;
        public SanPhamController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var ds = await _db.SanPhams.ToListAsync();
            return View(ds);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SanPham sp)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login", "Account");
            _db.SanPhams.Add(sp);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login", "Account");
            var sp = await _db.SanPhams.FindAsync(id);
            return View(sp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SanPham sp)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login", "Account");
            _db.SanPhams.Update(sp);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
                return RedirectToAction("Login", "Account");
            var sp = await _db.SanPhams.FindAsync(id);
            if (sp != null) _db.SanPhams.Remove(sp);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}