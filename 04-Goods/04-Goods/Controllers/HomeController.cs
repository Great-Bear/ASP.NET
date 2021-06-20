using _04_Goods.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _04_Goods.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListProduct()
        {
            var listProduct = new List<Product>();
            listProduct.Add(new Product(1, "Product 1", 120));
            listProduct.Add(new Product(2, "Product 2", 120));
            listProduct.Add(new Product(3, "Product 3", 120));

            listProduct.Add(new Product(4, "Product 4", 120));
            listProduct.Add(new Product(5, "Product 5", 120));
            listProduct.Add(new Product(6, "Product 6", 120));

            listProduct.Add(new Product(7, "Product 7", 120));
            listProduct.Add(new Product(8, "Product 8", 120));
            listProduct.Add(new Product(9, "Product 9", 120));

            listProduct.Add(new Product(10, "Product 10", 120));
            listProduct.Add(new Product(11, "Product 11", 120));
            listProduct.Add(new Product(12, "Product 12", 120));

            listProduct.Add(new Product(13, "Product 13", 120));
            listProduct.Add(new Product(14, "Product 14", 120));
            listProduct.Add(new Product(15, "Product 15", 120));

            return View(listProduct);
        }
        public IActionResult TableProduct()
        {
            var listProduct = new List<Product>();
            listProduct.Add(new Product(1, "Product 1", 120));
            listProduct.Add(new Product(2, "Product 2", 120));
            listProduct.Add(new Product(3, "Product 3", 120));

            listProduct.Add(new Product(4, "Product 4", 120));
            listProduct.Add(new Product(5, "Product 5", 120));
            listProduct.Add(new Product(6, "Product 6", 120));

            listProduct.Add(new Product(7, "Product 7", 120));
            listProduct.Add(new Product(8, "Product 8", 120));
            listProduct.Add(new Product(9, "Product 9", 120));

            listProduct.Add(new Product(10, "Product 10", 120));
            listProduct.Add(new Product(11, "Product 11", 120));
            listProduct.Add(new Product(12, "Product 12", 120));

            listProduct.Add(new Product(13, "Product 13", 120));
            listProduct.Add(new Product(14, "Product 14", 120));
            listProduct.Add(new Product(15, "Product 15", 120));

            return View(listProduct);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
