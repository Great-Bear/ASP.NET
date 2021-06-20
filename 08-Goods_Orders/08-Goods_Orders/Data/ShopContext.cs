
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _08_Goods_Orders.Models;

namespace _08_Goods_Orders.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext (DbContextOptions<ShopContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }     
        public DbSet<Order> Order { get; set; }
    }
}
