using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08_Goods_Orders.Models
{
    public class Shop
    {
        public Order Orders { get; set; }
        public User Users { get; set; }
        public Product Products { get; set; }
    }
}
