using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goods.Models
{
    public class ProductsList
    {
        public List<Product> Items { get; set; } = new List<Product>();
        public ProductsList()
        {
            Product._id = -1;
            Items.Add(new Product("Single funny cookies", "This is signle cookie", "~/imgs/SingleFunnyCookis.jpg"));
            Items.Add(new Product("Two funny cookies", "This is two cookies", "~/imgs/TwoFunnyCookies.jpg"));
            Items.Add(new Product("Many funny cookies", "This is many cookies", "~/imgs/ManyfunnyCookies.jpg"));
            Items.Add(new Product("Cookies with mastic", "This is mastic cookies", "~/imgs/СookiesWithMastic.jpg"));
            Items.Add(new Product("Cookies with chocolate", "This is chocolate cookies", "~/imgs/CookiesWithСhocolate.jpg"));
            Items.Add(new Product("Cookies with jelly", "This is jelly cookies", "~/imgs/CookiesWithJelly.jpg"));
            Items.Add(new Product("Cookies black-white", "This is black-white cookies", "~/imgs/CookiesBlackWhite.jpg"));
            Items.Add(new Product("Cookies black", "This is black cookies", "~/imgs/CookiesBlack.jpg"));
        }
    }
}
