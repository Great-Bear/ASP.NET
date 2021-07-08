using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int? RoleId { get; set; } 
        public Role Role { get; set; }

        public List<Goods> Goods { get; set; }
        public List<Order> Orders { get; set; }
    }
}
