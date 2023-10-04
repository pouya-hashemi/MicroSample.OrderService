using MicroSample.OrderService.Entities;
using MicroSample.OrderService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroSample.OrderService.Persistance
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
    }
}
