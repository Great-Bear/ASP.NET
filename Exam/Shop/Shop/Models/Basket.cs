﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Basket
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public float Price { get; set; }
        public string Picture { get; set; }
    }
}
