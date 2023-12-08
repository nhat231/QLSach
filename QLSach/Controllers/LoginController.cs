using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QLSach.Models;
using System.Security.Claims;

namespace QLSach.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _appdbcontext;
        public IHttpContextAccessor _contextAccessor;
        public LoginController(AppDbContext appdbcontext, IHttpContextAccessor contextAccessor)
        {
            _appdbcontext = appdbcontext;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            if (string.IsNullOrEmpty(modelLogin.MANV))
            {
                TempData["Message"] = "Tên đăng nhập không được trống.";
                return RedirectToAction("Login");
            }
            if (string.IsNullOrEmpty(modelLogin.MATKHAU))
            {
                TempData["Message"] = "Mật khẩu không được trống.";
                return RedirectToAction("Login");
            }


            var rowUser =  _appdbcontext.NHANVIENS.FirstOrDefault(x => x.MANV == modelLogin.MANV && x.MATKHAU == modelLogin.MATKHAU);
            if (rowUser == null)
            {
                TempData["Message"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return RedirectToAction("Login");
            }

            List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.MANV),
                    new Claim("OtherProperties","Example Role")

                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            _contextAccessor.HttpContext.Session.SetString("Username", modelLogin.MANV);

            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
