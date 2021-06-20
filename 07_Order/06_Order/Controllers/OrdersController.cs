using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _06_Order.Models;
using System.Data.SqlClient;

namespace _06_Order.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;             
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }
      
        public IActionResult Create()
        {
            ViewBag.MinDate = DateTime.Now.GetDateTimeFormats()[4];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Email,Date,TermsAccepted")] Order order)
        {
            if (ModelState.IsValid)
            {
                var Name = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Name",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                var Address = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Address",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                }; 
                var Email = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                var Date = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Date",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                };
                var TermsAccepted = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@TermsAccepted",
                    SqlDbType = System.Data.SqlDbType.Bit,
                };

                Name.Value = order.Name;
                Address.Value = order.Address;
                Email.Value = order.Email;
                Date.Value = order.Date;
                TermsAccepted.Value = order.TermsAccepted;

                 _context.Database.ExecuteSqlRaw("AddOrder @Name, @Address, @Email, @Date, @TermsAccepted", Name, Address, Email, Date, TermsAccepted);

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
       
    }
}
