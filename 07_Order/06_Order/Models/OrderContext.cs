using Microsoft.EntityFrameworkCore;
using System;

namespace _06_Order.Models
{
    public class OrderContext : DbContext { 
    
        public DbSet<Order> Orders { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {        
            if (Database.EnsureCreated()){
                FormattableString Execute = $"CREATE PROCEDURE AddOrder @Name NVARCHAR(50), @Address NVARCHAR(50), @Email NVARCHAR(50), @Date DateTime2, @TermsAccepted bit AS INSERT INTO Orders(Name, Address, Email, Date, TermsAccepted) VALUES(@Name, @Address, @Email, @Date, @TermsAccepted)";
                Database.ExecuteSqlInterpolated(Execute);
            }
        }
           
    }
}
