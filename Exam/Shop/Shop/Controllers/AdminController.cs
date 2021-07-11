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
    public class AdminController : Controller
    {
        private readonly ShopContext _context;
        public AdminController(ShopContext context)
        {
            _context = context;
        }


        public IActionResult ListOrders(string stateOrder = null)
        {
            List<Order> Orders;
            if (stateOrder != null && stateOrder != "All")
            {
                Orders = _context.Orders.Include(order => order.Good).Include(order => order.StateOrder).
                                         Where(order => order.StateOrder.Name == stateOrder).ToList();
            }
            else
            {
                Orders = _context.Orders.Include(order => order.Good).Include(order => order.StateOrder).ToList();
            }
          
            ViewBag.StatesOrder = _context.StateOrders.ToList();
            foreach (var item in Orders)
            {
                if(item.Good.IsLoadPicture == false) 
                {
                    LoadPicture(item.Good);
                }
                   
            }

            return View(Orders);
        }
        public IActionResult Users()
        {
            var ListUsers = _context.Users.Include(user => user.Role).ToList();
            return View(ListUsers);
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
                goods.IsLoadPicture = true;
            }
        }
    }
}
