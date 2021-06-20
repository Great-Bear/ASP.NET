using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _08_Goods_Orders.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Describe { get; set; }
        public DateTime DateCreate { get; set; }
        [NotMapped]
        public string DateCreateS => $"{DateCreate.Day}.{DateCreate.Month}.{DateCreate.Year}"; 
        public ICollection<Order> Order { get; set; }

    }
}
