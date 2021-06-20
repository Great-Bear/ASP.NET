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
    public class OrdersController : Controller
    {
        private readonly ShopContext _context;

        public OrdersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Order.Include(o => o.Product).Include(o => o.User);
            var ordersList = shopContext.ToList();
            ViewBag.Sum = ordersList.Sum(item => item.CountProducts * item.Product.Price);
            if (ordersList.Count != 0)
            {
                ViewBag.firstDate = ordersList.Min(item => item.DateBuy).GetDateTimeFormats()[4];
                ViewBag.secondDate = ordersList.Max(item => item.DateBuy).GetDateTimeFormats()[4];
            }
            return View(await shopContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateTime firstDate, DateTime secondDate)
        {
            var shopContext = _context.Order.Include(o => o.Product).Include(o => o.User);
            var ordersList = shopContext.ToList();
            ViewBag.firstDate = firstDate.GetDateTimeFormats()[4];
            ViewBag.secondDate = secondDate.GetDateTimeFormats()[4];
            var fg = ordersList.Where(item => item.DateBuy.Date >= firstDate.Date && item.DateBuy.Date <= secondDate.Date).ToList();
            ViewBag.Sum = ordersList.Sum(item => item.CountProducts * item.Product.Price);
            return View(fg);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.CurrentDate = DateTime.Now.GetDateTimeFormats()[4];
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name");
            ViewData["UserID"] = new SelectList(_context.User, "ID", "SurName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CountProducts,DateBuy,UserID,ProductID")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", order.Product);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "SurName", order.User);
            return View(order);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", order.ProductID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "SurName", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CountByuProd,DateBuy,UserID,ProductID")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", order.ProductID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", order.UserID);
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID == id);
        }
    }
}
