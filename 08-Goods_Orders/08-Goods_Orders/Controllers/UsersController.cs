using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _08_Goods_Orders.Data;
using _08_Goods_Orders.Models;

namespace _08_Goods_Orders.Controllers
{
    public class UsersController : Controller
    {
        private readonly ShopContext _context;

        public UsersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }
        public async Task<IActionResult> History(string valueSearch)
        {
            var shopContext = _context.Order.Include(o => o.Product).Include(o => o.User);
            var ordersList = shopContext.ToList();
            var PersonalOrdersList = ordersList.ToList();
            if (valueSearch != null)
            {
                PersonalOrdersList = ordersList.Where(item => item.User.SurName == valueSearch).ToList();
                int idUser;
                if(PersonalOrdersList.Count == 0)
                {
                    PersonalOrdersList = ordersList.Where(item => item.User.Name == valueSearch).ToList();
                }
                if (int.TryParse(valueSearch,out idUser) == true)
                {
                    PersonalOrdersList = ordersList.Where(item => item.UserID == idUser).ToList();
                }
            }
            ViewBag.Sum = PersonalOrdersList.Sum(item => item.CountProducts * item.Product.Price);
            return View(PersonalOrdersList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public IActionResult Create()
        {
            ViewBag.CurrentDate = DateTime.Now.GetDateTimeFormats()[4];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,SurName,DateCreate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,SurName,DateCreate")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
