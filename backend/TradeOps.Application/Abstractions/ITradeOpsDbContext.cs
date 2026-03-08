using Microsoft.EntityFrameworkCore;
using TradeOps.Domain.Entities;

namespace TradeOps.Application.Abstractions;

public interface ITradeOpsDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}