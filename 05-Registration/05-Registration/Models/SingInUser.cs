using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05_Registration.Models
{
    [Serializable]
    public class SingInUser : LogInUser
    {
        public string Password2 { get; set; }
        public string Email { get; set; }
        public SingInUser()
        {

        }
        public SingInUser(string login, string password, string password2,string email) : base(login, password)
        {
            Password2 = password2;
            Email = email;
        }
    }
}
