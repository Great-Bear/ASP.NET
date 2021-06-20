using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerShop.Models
{
    public class Order
    {
        public string NameFlover { get; set; }
        public int Count { get; set; }
        public Address DeliveryAddress { get; set; }
        public string TextNote { get; set; }
        public string NameOrder { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string FullDate => $"{DeliveryDate.Year} / {DeliveryDate.Month} / {DeliveryDate.Day}";
        public Order(string nameFlover,int count,Address deliveryAddress,string textNote,string nameOrder,DateTime deliveryDate)
        {
            NameFlover = nameFlover;
            Count = count;
            DeliveryAddress = deliveryAddress;
            TextNote = textNote;
            NameOrder = nameOrder;
            DeliveryDate = deliveryDate;
        }
    }
}
