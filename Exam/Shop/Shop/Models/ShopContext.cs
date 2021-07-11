using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Models
{
    public class ShopContext : DbContext
    {
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StateOrder> StateOrders { get; set; }
        public DbSet<TypeGood> TypeGoods { get; set; }

        public ShopContext (DbContextOptions<ShopContext> options)
            : base(options)
        {
            Database.EnsureCreated();           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
