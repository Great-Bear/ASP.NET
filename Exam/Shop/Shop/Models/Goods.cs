using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Goods
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Describe { get; set; }
        [Required]
        public float Price { get; set; }
        [NotMapped]
        public bool IsLoadPicture { get; set; } = false;    
        public bool State { get; set; } = true;

        public int TypeId { get; set; }
        public TypeGood Type { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }

        public string Picture { get; set; }
    }
}
