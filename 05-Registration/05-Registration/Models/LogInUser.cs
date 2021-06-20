using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05_Registration.Models
{
    [Serializable]
    public class LogInUser
    {
        public DateTime DateLogIN { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public LogInUser()
        {
            DateLogIN = DateTime.Now;
        }
        public LogInUser(string login, string password) : this()
        {
            Login = login;
            Password = password;
        }
    }
}
