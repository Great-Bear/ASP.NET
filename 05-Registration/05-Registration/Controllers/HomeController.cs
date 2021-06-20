using _05_Registration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace _05_Registration.Controllers
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
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult LogIN()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> LogIN(string login, string password)
        {
            var user = new LogInUser(login, password);

            return await WriteLogInUserAsync(user);
        }
        private async Task<string> WriteLogInUserAsync(LogInUser user)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<LogInUser> ListUsers = null;

            using (FileStream fs = new FileStream("UsersLogIN.dat", FileMode.OpenOrCreate))
            {
                if(fs.Length != 0)
                ListUsers = (List<LogInUser>)formatter.Deserialize(fs);
            }

            if (ListUsers == null)
            {
                ListUsers = new List<LogInUser>();
            }
            ListUsers.Add(user);

            using (FileStream fs = new FileStream("UsersLogIN.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListUsers);
            }

            return "Access operation";
        }
        public IActionResult SingIN()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> SingIN(string login, string password, string password2, string email)
        {
            var user = new SingInUser(login, password, password2, email);
            
            return await WriteSingInUserAsync(user);
        }

        private async Task<string> WriteSingInUserAsync(SingInUser user)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<SingInUser> ListUsers = null;

            using (FileStream fs = new FileStream("UsersSingIN.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                    ListUsers = (List<SingInUser>)formatter.Deserialize(fs);
            }

            if (ListUsers == null)
            {
                ListUsers = new List<SingInUser>();
            }
            ListUsers.Add(user);

            using (FileStream fs = new FileStream("UsersSingIN.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListUsers);
            }

            return "Access opearation";
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult Privacy()
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
