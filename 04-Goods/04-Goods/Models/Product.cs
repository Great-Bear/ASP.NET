using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _04_Goods.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public Product(int id, string name,int price, DateTime createDate = new DateTime())
        {
            ID = id;
            Name = name;
            Price = price;
            CreatedDate = createDate;
        }
    }
}
