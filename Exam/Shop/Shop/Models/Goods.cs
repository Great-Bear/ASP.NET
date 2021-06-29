using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Goods
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public float Price { get; set; }
        [NotMapped]
        public string Picture { get; set; }
    }
}
