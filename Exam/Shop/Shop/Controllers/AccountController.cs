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


                Users user = _context.Users
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

                Users user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == passwordHash);

                if (user == null)
                {
                    user = new Users
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
        private async Task Authenticate(Users user)
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

        public IActionResult Edit(int IdUser) 
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == IdUser);

            ViewBag.CurrentRoleId = _context.Roles.FirstOrDefault(role => role.Id == user.RoleId).Id;
            ViewData["IDM"] = _context.Roles.FirstOrDefault(role => role.Id == user.RoleId).Id;

            ViewBag.List = _context.Roles.ToList();



            return View(user);
        }
        public IActionResult EditRole(string Role, int IdUser)
        {
            _context.Users.FirstOrDefault(user => user.Id == IdUser).RoleId =
            _context.Roles.FirstOrDefault(role => role.Name == Role).Id;
            _context.SaveChanges();

            return RedirectToAction("Users","Admin");
        }
    }
}
