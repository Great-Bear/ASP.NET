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
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role { Name = "user" });
                _context.Roles.Add(new Role { Name = "admin" });
            }
            _context.SaveChanges();
            int idUserTest = 2;
            if (!_context.Users.Any())
            {
                var idAdminRole = _context.Roles.FirstOrDefault(Role => Role.Name == "admin").Id;
                var idUserRole = _context.Roles.FirstOrDefault(Role => Role.Name == "user").Id;

                _context.Users.Add(new Users { Email = "admin@admin.com", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", RoleId = idAdminRole });
                _context.Users.Add(new Users { Email = "user@user.com", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", RoleId = idUserRole });
                _context.SaveChanges();
                idUserTest = _context.Users.FirstOrDefault(user => user.Email == "user@user.com").Id;
            }
            _context.SaveChanges();


            if (!_context.TypeGoods.Any())
            {
                _context.Add(new TypeGood { Name = "Cookies" });
                _context.Add(new TypeGood { Name = "Enimals" });
                _context.Add(new TypeGood { Name = "Cars" });
            }
            _context.SaveChanges();
            int IdTypeCookie = _context.TypeGoods.FirstOrDefault(TypeGood => TypeGood.Name == "Cookies").ID;
            int IdTypeEnimal = _context.TypeGoods.FirstOrDefault(TypeGood => TypeGood.Name == "Enimals").ID;
            int IdTypeCar = _context.TypeGoods.FirstOrDefault(TypeGood => TypeGood.Name == "Cars").ID;

            if (!_context.Goods.Any())
            {
                _context.Goods.Add(new Goods
                {
                    Name = "Cookie 1",
                    Describe = "Descrie text",
                    Price = 123,
                    UserId = idUserTest,
                    Picture = "Cookie1.jpg",
                    TypeId = IdTypeCookie

                });
                _context.Goods.Add(new Goods
                {
                    Name = "Cookie 2",
                    Describe = "Descrie text",
                    Price = 123,
                    UserId = idUserTest,
                    Picture = "Cookie2.jpg",
                    TypeId = IdTypeCookie

                });
                _context.Goods.Add(new Goods
                {
                    Name = "Cookie 3",
                    Describe = "Descrie text",
                    Price = 123,
                    UserId = idUserTest,
                    Picture = "Cookie3.jpg",
                    TypeId = IdTypeCookie
                });
                _context.Goods.Add(new Goods
                {
                    Name = "Car 1",
                    Describe = "Descrie text",
                    Price = 123,
                    UserId = idUserTest,
                    Picture = "Car1.jpg",
                    TypeId = IdTypeCar
                });
                _context.Goods.Add(new Goods
                {
                    Name = "Car 1",
                    Describe = "Descrie text",
                    Price = 123,
                    UserId = idUserTest,
                    Picture = "Penguine1.jpg",
                    TypeId = IdTypeEnimal
                });
            }
            _context.SaveChanges();

            if (!_context.StateOrders.Any())
            {
                _context.Add(new StateOrder
                {
                    Name = "Waiting"
                });
                _context.Add(new StateOrder
                {
                    Name = "Canceled"
                });
                _context.Add(new StateOrder
                {
                    Name = "Doing"
                });
                _context.Add(new StateOrder
                {
                    Name = "Done"
                });
            }
        }         

        public async Task<IActionResult> ListGoods(string sortName = "", string valueSearch = null)
        {
            ViewBag.TypeGoods = _context.TypeGoods.ToList();
            if(valueSearch != null)
            {
                var resultList = _context.Goods.Include(good => good.Type).Where(Good => Good.Name.Contains(valueSearch)).ToList();
                foreach (var item in resultList)
                {
                    LoadPicture(item);
                }
                return View(resultList);
            }

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
                var GoodsLsit = await _context.Goods.Where(good => good.Type.Name == sortName).ToListAsync();
                foreach (var item in GoodsLsit)
                {
                   LoadPicture(item);
                }
                return View(GoodsLsit);
            }
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

        public async Task<IActionResult> Details(int? id, bool fromBasket = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.FromBasket = false;
            var goods = await _context.Goods.Include(good => good.Type)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (goods == null)
            {
                return NotFound();
            }
            if (fromBasket)
            {
                ViewBag.FromBasket = true;
            }
            LoadPicture(goods);

            return View(goods);
        }

        public IActionResult Create()
        {
            ViewBag.TypeGoods = _context.TypeGoods.ToList();
            return View();
        }


        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Describe,Price,Type")] Goods goods, IFormFile uploadedFile, string typeName)
        {          
            if (ModelState.IsValid)
            {
                var listClaims = HttpContext.User.Claims.ToList();
                goods.UserId = int.Parse(listClaims[2].Value);
                goods.Picture = goods.UserId.ToString() + uploadedFile.FileName;
                var newTypeID = _context.TypeGoods.FirstOrDefault(Type => Type.Name == typeName).ID;
                goods.TypeId = newTypeID;
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.TypeGoods = _context.TypeGoods.ToList();
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Describe,Price,State")] Goods goods, IFormFile uploadedFile,string typeName)
        {
            if (id != goods.ID)
            {
                return NotFound();
            }

            var newTypeID = _context.TypeGoods.FirstOrDefault(Type => Type.Name == typeName).ID;
            var newState = goods.State;
            var oldGood = goods;

            goods = _context.Goods.FirstOrDefault(good => good.ID == goods.ID);

            goods.State = newState;
            goods.TypeId = newTypeID;
            goods.Name = oldGood.Name;
            goods.Describe = oldGood.Describe;
            goods.Price = oldGood.Price;
            goods.State = oldGood.State;

            if (ModelState.IsValid)
            {   
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
