using MicroSample.OrderService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroSample.OrderService.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    }
}
