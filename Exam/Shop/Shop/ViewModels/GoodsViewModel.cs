using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class GoodsViewModel
    {
        public string Name { get; set; }
        public string Describe { get; set; }
        public float Price { get; set; }
        public int UserId { get; set; }
        public FormFile Picture { get; set; }
    }
}
