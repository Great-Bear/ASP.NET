using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
         public IActionResult Index(int firstNum, string oper, int secondNum)
         {
             string authData = "Result:";
             switch (oper)
             {
                 case "Plus":
                     authData += (firstNum + secondNum).ToString();
                     break;
                 case "Remove":
                     authData += (firstNum - secondNum).ToString();
                     break;
                 case "Divide":
                     authData += (firstNum / secondNum).ToString();
                     break;
                 case "Multiply":
                     authData += (firstNum * secondNum).ToString();
                     break;
             }
             return Content(authData);
         }

    }
}
