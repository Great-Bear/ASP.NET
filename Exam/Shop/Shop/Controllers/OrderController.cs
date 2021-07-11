using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopContext _context;

        public OrderController(ShopContext context)
        {
            _context = context;
        }

        public IActionResult Edit(int idOrder)
        {
            var order = _context.Orders.Include(order => order.Good).Include(order => order.StateOrder).FirstOrDefault(order => order.ID == idOrder);

            ViewBag.ListStatesOrder = _context.StateOrders.ToList();
            LoadPicture(order.Good);
            return View(order);
        }
        public IActionResult EditStateOrder(int idOrder,int newStateOrderId)
        {
            _context.Orders.FirstOrDefault(Order => Order.ID == idOrder).StateOrderId = newStateOrderId;
            _context.SaveChanges();

            return RedirectToAction("listOrders", "Admin");
        }
        public void LoadPicture(Goods goods)
        {
            string fileName = goods.Picture;
            if (fileName == null || System.IO.File.Exists(@$".\Imgs\{fileName}") == false)
            {
                fileName = "empty.jpg";
            }
            var picture = System.IO.File.Open(@$".\Imgs\{fileName}", FileMode.Open);

            using (var binaryReader = new BinaryReader(picture))
            {
                goods.Picture = "data:image / jpeg; base64," +
                                Convert.ToBase64String(binaryReader.ReadBytes((int)picture.Length));
            }
        }
    }
}
