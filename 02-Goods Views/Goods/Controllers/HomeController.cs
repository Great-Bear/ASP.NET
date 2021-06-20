using Goods.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Goods.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ProductsList ProductsList = new ProductsList();
        public HomeController(ILogger<HomeController> logger)
        {         
            _logger = logger;
            ProductsList = new ProductsList();
        }      
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Header()
        {
            return PartialView();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View(ProductsList.Items);
        }
        public IActionResult SpecificProduct(int id)
        {
            var a = ProductsList.Items.Where(prod => prod.ID == id).ToList();
            return View(ProductsList.Items.Where(prod => prod.ID == id).ToList()[0]);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
