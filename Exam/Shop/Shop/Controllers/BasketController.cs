using CookiesAndSessions.Extentions;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class BasketController : Controller
    {

        private readonly ShopContext _context;

        public BasketController(ShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Basket()
        {
            var baket = (List<Goods>)HttpContext.Session.GetObject("basket", typeof(List<Goods>));
            foreach (var item in baket)
            {
                    LoadPicture(item);
            }

            return View(baket);
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

        public void Buy(int idGood) 
        {
            var listClaims = HttpContext.User.Claims.ToList();
            int idUser = int.Parse(listClaims[2].Value);
            _context.Orders.Add(new Order { GoodId = idGood, UserId = idUser });
        }
    }

    
}
