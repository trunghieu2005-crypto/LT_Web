using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KT_GK.Controllers
{
        public class AccountController : Controller
        {
            // Sử dụng SignInManager của Identity để quản lý đăng nhập
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly UserManager<IdentityUser> _userManager;

            public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
            {
                _signInManager = signInManager;
                _userManager = userManager;
            }

            // GET: /Account/Login
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            // POST: /Account/Login
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(string MaSV, string Password)
            {
                if (ModelState.IsValid)
                {
                    // Identity dùng UserName để đăng nhập, ở đây ta coi MaSV là UserName
                    var result = await _signInManager.PasswordSignInAsync(MaSV, Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Đăng nhập thành công, chuyển hướng sang trang danh sách học phần (Câu 3)
                        return RedirectToAction("Index", "HocPhan");
                    }

                    ModelState.AddModelError(string.Empty, "Mã sinh viên hoặc mật khẩu không chính xác.");
                }

                return View();
            }

            // POST: /Account/Logout
            [HttpPost]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
        }
}
