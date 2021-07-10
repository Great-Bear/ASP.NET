using CookiesAndSessions.Extentions;
using Microsoft.AspNetCore.Http;
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
    public class BasketController : Controller
    {

        private readonly ShopContext _context;

        public BasketController(ShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Basket()
        {
            var basket = (List<Goods>)HttpContext.Session.GetObject("basket", typeof(List<Goods>));

            if (basket == null)
            {
                basket = new List<Goods>();
            }

            foreach (var item in basket)
            {
                    LoadPicture(item);
            }      
            return View(basket);
        }
        public IActionResult DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods =  _context.Goods
                .FirstOrDefault(m => m.ID == id);
            if (goods == null)
            {
                return NotFound();
            }
            LoadPicture(goods);

            return View(goods);
        }
        public IActionResult DeleteAll()
        {
            var basket = (List<Goods>)HttpContext.Session.GetObject("basket", typeof(List<Goods>));
            foreach (var item in basket)
            {
                LoadPicture(item);
            }
            return View(basket);
        }

        public IActionResult DeleteConfirmed(int id, bool all = false)
        {
            if (all) 
            {
                HttpContext.Session.SetObject("basket", new List<Goods>(), typeof(List<Goods>));
                HttpContext.Session.SetInt32("basketLength", 0);     
                return RedirectToAction("ListGoods", "Goods");
            }

            RemoveItemFormBasket(id);

            return RedirectToAction(nameof(Basket));
        }

        public IActionResult Buy(int idGood)
        {
            var listClaims = HttpContext.User.Claims.ToList();
            int idUser = int.Parse(listClaims[2].Value);
            _context.Orders.Add(new Order { GoodId = idGood, UserId = idUser });
            _context.SaveChanges();

            RemoveItemFormBasket(idGood);

            return RedirectToAction(nameof(Basket));
        }

        private void RemoveItemFormBasket(int idItem) 
        {
            var basket = (List<Goods>)HttpContext.Session.GetObject("basket", typeof(List<Goods>));
            basket.RemoveAll(item => item.ID == idItem);

            HttpContext.Session.SetObject("basket", basket, typeof(List<Goods>));
            var lengthBasket = HttpContext.Session.GetInt32("basketLength");
            HttpContext.Session.SetInt32("basketLength", (int)--lengthBasket);
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
