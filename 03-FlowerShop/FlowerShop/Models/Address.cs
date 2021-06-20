using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Models
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int NumberHome { get; set; }
        public string FullAdress => $"{City}/{Street}/{NumberHome}";
        public Address(string city,string street,int numberHome)
        {
            City = city;
            Street = street;
            NumberHome = numberHome;
        }
    }
}
