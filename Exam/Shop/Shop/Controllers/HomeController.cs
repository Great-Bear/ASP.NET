using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        public IActionResult MainPage()
        {
            _logger.LogInformation("Test info");    
            return View();
        }
        public IActionResult TestPicture()
        {
            var a = Directory.GetFiles(@".\Imgs\");
            for(int i = 0; i < a.Length; i++)
            {
                var file =  System.IO.File.Open(a[i],FileMode.Open);
                using (var binaryReader = new BinaryReader(file))
                {
                    a[i] = "data:image / jpeg; base64," +
                                    Convert.ToBase64String(binaryReader.ReadBytes((int)file.Length));
                }
            }

            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = @".\Imgs\" + uploadedFile.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            return RedirectToAction("MainPage");
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
