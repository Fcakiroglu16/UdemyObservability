using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<OrderServices.Order> Orders { get; set; }
        public DbSet<OrderServices.OrderItem> OrderItems { get; set; }
    }
}
