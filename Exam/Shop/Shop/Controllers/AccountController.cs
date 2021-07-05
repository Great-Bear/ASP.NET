using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

                User user = _context.Users
                    .Include(user => user.Role)
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);

                if (user != null)
                {
                  //  await Authenticate(user);
                    return RedirectToAction("Index", "Home");
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
    }
}
