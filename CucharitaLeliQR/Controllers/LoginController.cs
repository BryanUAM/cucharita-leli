using System.Security.Claims;
using CucharitaLeliQR.Data;
using CucharitaLeliQR.Helpers;
using CucharitaLeliQR.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CucharitaLeliQR.Controllers
{
    public class LoginController : Controller
    {
        private readonly SodaContext _context;

        public LoginController(SodaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Usuario == model.Usuario);

            if (admin == null || !PasswordHelper.VerifyPassword(admin.PasswordHash, model.Password))
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Usuario),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Dashboard", "Clientes");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}