using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using KT_GK.Models;

namespace KT_GK.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SinhVien
        public async Task<IActionResult> Index()
        {
            // Sửa thành NganhHoc cho khớp với tên mới đặt
            var sinhViens = _context.SinhViens.Include(s => s.NganhHoc);
            return View(await sinhViens.ToListAsync());
        }

        // GET: SinhVien/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(m => m.MaSV == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // GET: SinhVien/Create
        [Authorize]
        public IActionResult Create()
        {
            // Load list of NganhHoc for dropdown
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh");
            return View();
        }

        // POST: SinhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MaSV,HoTen,GioiTinh,NgaySinh,MaNganh")] SinhVien sinhVien, IFormFile fileUpload)
        {
            // Xóa kiểm tra lỗi của trường Hinh và NganhHoc để ModelState không bị false vô lý
            // Nếu model có navigation property NganhHoc sẽ gây ra ModelState invalid khi binding
            ModelState.Remove("Hinh");
            ModelState.Remove("NganhHoc");

            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", "images");
                    if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);
                    var path = Path.Combine(imagesFolder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(stream);
                    }
                    sinhVien.Hinh = "/Content/images/" + fileName;
                }

                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Nếu lỗi, phải load lại danh sách ngành cho DropdownList
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // GET: SinhVien/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            // Pass list of NganhHoc for dropdown
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("MaSV,HoTen,GioiTinh,NgaySinh,MaNganh,Hinh")] SinhVien sinhVien, IFormFile fileUpload)
        {
            if (id != sinhVien.MaSV) return NotFound();

            // Loại bỏ validation cho navigation property và file input để tránh ModelState invalid
            ModelState.Remove("fileUpload");
            ModelState.Remove("NganhHoc");
            ModelState.Remove("Hinh");

            // Kiểm tra tồn tại bằng AsNoTracking để đảm bảo bản ghi có trong DB
            var existingNoTrack = await _context.SinhViens.AsNoTracking().FirstOrDefaultAsync(s => s.MaSV == id);
            if (existingNoTrack == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }

            // Lấy thực thể đang được track để cập nhật (tránh lỗi "instance is already being tracked")
            var existing = await _context.SinhViens.FindAsync(id);
            if (existing == null) return NotFound();

            // Xử lý ảnh: nếu upload mới thì xóa ảnh cũ (nếu có), lưu ảnh mới và cập nhật đường dẫn
            if (fileUpload != null && fileUpload.Length > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", "images");
                // Tạo thư mục nếu chưa tồn tại (tránh DirectoryNotFoundException)
                Directory.CreateDirectory(imagesFolder);

                // Xóa file cũ nếu có
                if (!string.IsNullOrEmpty(existing.Hinh))
                {
                    try
                    {
                        var oldRelative = existing.Hinh.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                        var oldPhysical = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldRelative);
                        if (System.IO.File.Exists(oldPhysical)) System.IO.File.Delete(oldPhysical);
                    }
                    catch
                    {
                        // Không dừng xử lý nếu xóa file cũ thất bại
                    }
                }

                var path = Path.Combine(imagesFolder, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(stream);
                }
                existing.Hinh = "/Content/images/" + fileName;
            }
            // Nếu không upload thì giữ nguyên existing.Hinh

            // Manual mapping các trường từ model vào thực thể đã load
            existing.HoTen = sinhVien.HoTen;
            existing.GioiTinh = sinhVien.GioiTinh;
            existing.NgaySinh = sinhVien.NgaySinh;
            existing.MaNganh = sinhVien.MaNganh;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinhVienExists(sinhVien.MaSV)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SinhVien/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(m => m.MaSV == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhViens.Any(e => e.MaSV == id);
        }
    }
}
