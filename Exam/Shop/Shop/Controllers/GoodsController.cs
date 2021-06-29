﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.IO;


namespace Shop.Controllers
{
    public class GoodsController : Controller
    {
        private readonly ShopContext _context;

        public GoodsController(ShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ListGoods()
        {
            string defaultPicture = string.Empty;
            var pictureFile = System.IO.File.Open(@".\Imgs\empty.jpg", FileMode.Open);
            using (var binaryReader = new BinaryReader(pictureFile))
            {
                defaultPicture = "data:image / jpeg; base64," +
                                Convert.ToBase64String(binaryReader.ReadBytes((int)pictureFile.Length));
            }


            var GoodsLsit = await _context.Goods.ToListAsync();
            foreach (var item in GoodsLsit)
            {
                if(item.Picture == null)
                {
                    item.Picture = defaultPicture;
                }
            }

            return View(GoodsLsit);
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
            if(goods.Picture == null) 
            {
                var picture = System.IO.File.Open(@".\Imgs\empty.jpg", FileMode.Open);
                using (var binaryReader = new BinaryReader(picture))
                {
                    goods.Picture = "data:image / jpeg; base64," +
                                    Convert.ToBase64String(binaryReader.ReadBytes((int)picture.Length));
                }
            }
            
            return View(goods);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Describe,Price")] Goods goods)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListGoods));
            }
            return View(goods);
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
            return View(goods);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Describe,Price")] Goods goods)
        {
            if (id != goods.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
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

            return View(goods);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goods = await _context.Goods.FindAsync(id);
            _context.Goods.Remove(goods);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsExists(int id)
        {
            return _context.Goods.Any(e => e.ID == id);
        }
    }
}