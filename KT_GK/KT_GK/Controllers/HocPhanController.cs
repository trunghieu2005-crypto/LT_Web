using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // Đảm bảo đã cài Package Newtonsoft.Json
using KT_GK.Models;

namespace KT_GK.Controllers
{
    [Authorize] // Câu 3: Bắt buộc đăng nhập
    public class HocPhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HocPhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Câu 3: Hiển thị danh sách học phần
        public async Task<IActionResult> Index()
        {
            // 1. Lấy danh sách tất cả học phần từ DB
            var allHocPhans = await _context.HocPhans.ToListAsync();

            // 2. Lấy danh sách đã chọn từ Session (Giỏ hàng)
            var sessionData = HttpContext.Session.GetString("GioHang");
            var cart = string.IsNullOrEmpty(sessionData)
                ? new List<HocPhan>()
                : JsonConvert.DeserializeObject<List<HocPhan>>(sessionData);

            // 3. Đưa giỏ hàng vào ViewBag để hiển thị chung trang
            ViewBag.GioHang = cart;

            return View(allHocPhans);
        }

        // Câu 3: Xử lý khi nhấn nút "Chọn"
        public IActionResult ChonHocPhan(string id)
        {
            var hp = _context.HocPhans.Find(id);
            if (hp == null) return NotFound();

            // Lấy danh sách đã chọn từ Session
            var sessionData = HttpContext.Session.GetString("GioHang");
            List<HocPhan> cart = new List<HocPhan>();

            if (!string.IsNullOrEmpty(sessionData))
            {
                cart = JsonConvert.DeserializeObject<List<HocPhan>>(sessionData);
            }

            // Nếu môn này chưa có trong danh sách thì mới thêm vào
            if (!cart.Any(x => x.MaHP == id))
            {
                cart.Add(hp);
                // Lưu lại vào Session
                HttpContext.Session.SetString("GioHang", JsonConvert.SerializeObject(cart));
            }

            // Sau khi chọn xong, tự động nhảy sang trang Giỏ hàng (Câu 4)
            return RedirectToAction("Index");
        }

        // Câu 4: Trang hiển thị các học phần đã chọn (Giỏ hàng)
        public IActionResult XoaHocPhan(string id)
        {
            var sessionData = HttpContext.Session.GetString("GioHang");
            if (!string.IsNullOrEmpty(sessionData))
            {
                var cart = JsonConvert.DeserializeObject<List<HocPhan>>(sessionData);
                var itemToRemove = cart.FirstOrDefault(x => x.MaHP == id);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetString("GioHang", JsonConvert.SerializeObject(cart));
                }
            }
            return RedirectToAction("Index");
        }
    }
}