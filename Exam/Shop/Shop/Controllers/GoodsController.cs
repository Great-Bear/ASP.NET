using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using CookiesAndSessions.Extentions;
using System.Security.Cryptography;
using System.Text;
using Shop.ViewModels;
using System.Threading;

namespace Shop.Controllers
{
    public class GoodsController : Controller
    {
        private readonly ShopContext _context;
        public List<Goods> BasketItems { get; set; } = new List<Goods>();

        public GoodsController(ShopContext context)
        {        
            _context = context;
        }
        public async Task<IActionResult> ListGoods(string sortName = "")
        {
            if (sortName == "")
            {
                var GoodsLsit = await _context.Goods.ToListAsync();
                foreach (var item in GoodsLsit)
                {
                        LoadPicture(item);
                }
                if (HttpContext.Session.Keys.Contains("basketLength") == false)
                {
                    HttpContext.Session.SetInt32("basketLength", 0);
                }

                var listClaims = HttpContext.User.Claims.ToList();
                if (HttpContext.User.Claims.ToList().Count > 0)
                    ViewBag.IdCurrUser = int.Parse(listClaims[2].Value);

                ViewBag.basketLength = HttpContext.Session.GetInt32("basketLength");
                return View(GoodsLsit);
            }
            else 
            {
                var GoodsLsit = await _context.Goods.Where(good => good.Type == "type 4 ").ToListAsync();
                foreach (var item in GoodsLsit)
                {
                   LoadPicture(item);
                }
                return View(GoodsLsit);
            }
        }
        string TakeDefaultPicture()
        {
            string defaultPicture = string.Empty;
            var pictureFile = System.IO.File.Open(@".\Imgs\empty.jpg", FileMode.Open);
            using (var binaryReader = new BinaryReader(pictureFile))
            {
                defaultPicture = "data:image / jpeg; base64," +
                                Convert.ToBase64String(binaryReader.ReadBytes((int)pictureFile.Length));
            }
            return defaultPicture;
        }
        public async Task<IActionResult> Basket()
        {
            var baket = (List<Goods>)HttpContext.Session.GetObject("basket", typeof(List<Goods>));
            string defaultPicture = TakeDefaultPicture();
            foreach (var item in baket)
            {
                if (item.Picture == null)
                {
                    item.Picture = defaultPicture;
                }
            }

            return View(baket);
        }



        public IActionResult Buy(int? id)
        {
            int? basketLength = HttpContext.Session.GetInt32("basketLength");
            basketLength++;
            HttpContext.Session.SetInt32("basketLength", (int)basketLength++);

            var newBasketItem = _context.Goods
                .FirstOrDefault(goods => goods.ID == id);

            if(newBasketItem != null)
            {
              if(HttpContext.Session.GetObject("basket", typeof(List<Goods>)) == null) 
              {
                    HttpContext.Session.SetObject("basket", new List<Goods>(), typeof(List<Goods>));
              }
               var basket = (List<Goods>)HttpContext.Session.GetObject("basket",typeof(List<Goods>));
                basket.Add(newBasketItem);
                HttpContext.Session.SetObject("basket", basket, typeof(List<Goods>));
            }
            return RedirectToAction(nameof(ListGoods));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _context.Goods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (goods == null)
            {
                return NotFound();
            }
            LoadPicture(goods);

            return View(goods);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Describe,Price,Type")] Goods goods, IFormFile uploadedFile)
        {          
            if (ModelState.IsValid)
            {
                var listClaims = HttpContext.User.Claims.ToList();
                goods.UserId = int.Parse(listClaims[2].Value);
                goods.Picture = goods.UserId.ToString() + uploadedFile.FileName;
                _context.Add(goods);
                await _context.SaveChangesAsync();

                if (uploadedFile != null)
                {
                    string path = @".\Imgs\" + goods.UserId.ToString() + uploadedFile.FileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }

                return RedirectToAction(nameof(ListGoods));
            }
            return View(null);
        }
        private void LoadPicture(Goods goods) 
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _context.Goods.FindAsync(id);
            if (goods == null)
            {
                return NotFound();
            }
            LoadPicture(goods);
            return View(goods);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Describe,Price")] Goods goods, IFormFile uploadedFile)
        {
            if (id != goods.ID)
            {
                return NotFound();
            }
            goods = _context.Goods.FirstOrDefault(good => good.ID == goods.ID);

            if (ModelState.IsValid)
            {
                var listClaims = HttpContext.User.Claims.ToList();               
                goods.UserId = int.Parse(listClaims[2].Value);
                
                try
                {
                    if (uploadedFile != null)
                    { 
                       if(goods.Picture != "empty.jpg" && goods.Picture != null && System.IO.File.Exists(@$".\Imgs\{goods.Picture}") != false)
                          System.IO.File.Delete(@".\Imgs\" + goods.Picture);
                        
           
                        string path = @".\Imgs\" + goods.UserId.ToString() + uploadedFile.FileName;
                        goods.Picture = goods.UserId.ToString() + uploadedFile.FileName;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(goods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsExists(goods.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListGoods));
            }
            
            return View(goods);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var goods = await _context.Goods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (goods == null)
            {
                return NotFound();
            }
            LoadPicture(goods);

            return View(goods);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goods = await _context.Goods.FindAsync(id);

            if (goods.Picture != "empty.jpg" && goods.Picture != null && System.IO.File.Exists(@$".\Imgs\{goods.Picture}") != false)
               
            System.IO.File.Delete(@".\Imgs\" + goods.Picture);
               

            _context.Goods.Remove(goods);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListGoods));
        }

        private bool GoodsExists(int id)
        {
            return _context.Goods.Any(e => e.ID == id);
        }
    }
}
