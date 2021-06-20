using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _06_Order.Models;


namespace _06_Orders.Controllers
{
    public class OrdersController : Controller
    {

        public IActionResult Create()
        {
            ViewBag.MinDate = DateTime.Now.GetDateTimeFormats()[4];
            return View();
        }
        public IActionResult SuccessOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Address,Email,Date,TermsAccepted")] Order order)
        {
            if (ModelState.IsValid)
            {
                return View(nameof(SuccessOrder));
            }
            return View(order);
        }
    }
}
