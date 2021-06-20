using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goods
{
    public class Product
    {
        public static int _id { get; set; } = -1;
        public int ID { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public string PathPhoto { get; set; }
        public Product(string name,string describe, string pathPhoto) 
        {
            _id++;
            ID = _id;
            Name = name;
            Describe = describe;
            PathPhoto = pathPhoto.Substring(1,pathPhoto.Length - 1);
        }

    }
}
