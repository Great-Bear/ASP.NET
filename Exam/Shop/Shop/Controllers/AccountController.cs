
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {

        private readonly ShopContext _context;

        public AccountController(ShopContext context)
        {
            _context = context;
            var a = _context.Roles.ToList();
            var b = _context.Users.ToList();
            if(!_context.Roles.Any())
            {
                _context.Roles.Add(new Role { Name = "user" });
                _context.Roles.Add(new Role { Name = "admin" });
            }
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User { Email = "admin@admin.com", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", RoleId = 2 });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                var a = _context.Roles.ToList();
                var b = _context.Users.ToList();

                User user = _context.Users
                    .Include(user => user.Role)
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);

                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("ListGoods", "Goods");
                }

                ModelState.AddModelError("", "Invalid login or password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                User user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == passwordHash);

                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        Password = passwordHash,
                    };

                    Role roleUser = await _context.Roles
                        .FirstOrDefaultAsync(role => role.Name == "user");

                    if (roleUser != null)
                    {
                        user.Role = roleUser;
                        user.RoleId = roleUser.Id;
                    }

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Email is already taken");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync();
            return RedirectToAction("ListGoods", "Goods");
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("ID",user.Id.ToString()),
            };

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
             );

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
