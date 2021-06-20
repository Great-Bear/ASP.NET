using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Models
{
    public class OrdersList
    {
        public List<Order> Orders = new List<Order>();
        public OrdersList()
        {
            Orders.Add(new Order("Begonia", 21,new Address("Odessa","Malinovskogo 2",2),"it is for you","Vika",new DateTime(2021, 3, 7)));
            Orders.Add(new Order("Bromelia", 11, new Address("Odessa", "Sobornaya 5", 22), "it is for you", "Vova", new DateTime(2021, 3, 7)));
            Orders.Add(new Order("Alstroemeria", 11, new Address("Odessa", "Phontanskaya 5", 9), "it is for you", "Katya", new DateTime(2021, 3, 7)));
            
            Orders.Add(new Order("Brugmansia", 101, new Address("Odessa", "Pushkinskaya 12", 45), "it is for you", "Oksana", new DateTime(2021, 4, 5)));
            Orders.Add(new Order("African daisies", 17, new Address("Odessa", "Sobornaya 5", 22), "it is for you", "Vova", new DateTime(2021, 9, 23)));
            Orders.Add(new Order("White Lotus", 77, new Address("Odessa", "Phontanskaya 5", 22), "it is for you", "Katya", new DateTime(2021, 3, 7)));
        }
    }
}
