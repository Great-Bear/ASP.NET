using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _08_Goods_Orders.Models
{
    public class Order
    {
        public int ID { get; set; }
        [Required]
        public int CountProducts { get; set; }
        public DateTime DateBuy { get; set; }
        [NotMapped]
        public string DateBuyS => $"{DateBuy.Day}.{DateBuy.Month}.{DateBuy.Year}";
        [Required(ErrorMessage = "Field User is required")]
        public int? UserID { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Product User is required")]
        public int? ProductID { get; set; }
        public Product Product { get; set; }

    }
}
