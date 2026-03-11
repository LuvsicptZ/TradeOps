using Microsoft.EntityFrameworkCore;
using TradeOps.Application.Abstractions;
using TradeOps.Domain.Entities;

namespace TradeOps.Infrastructure.Persistence;

public sealed class TradeOpsDbContext : DbContext, ITradeOpsDbContext
{
    private readonly ICurrentTenant _currentTenant;

    public TradeOpsDbContext(
        DbContextOptions<TradeOpsDbContext> options,
        ICurrentTenant currentTenant) : base(options)
    {
        _currentTenant = currentTenant;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();

    public override int SaveChanges()
    {
        ApplyTenantToNewEntities();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyTenantToNewEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyTenantToNewEntities()
    {
        var tenantId = _currentTenant.TenantId;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Added)
                continue;

            switch (entry.Entity)
            {
                case User user:
                    if (!tenantId.HasValue)
                        throw new InvalidOperationException("无法新增用户：缺少租户上下文。");
                    user.TenantId = tenantId.Value;
                    break;
                case Customer customer:
                    if (!tenantId.HasValue)
                        throw new InvalidOperationException("无法新增客户：缺少租户上下文。");
                    customer.TenantId = tenantId.Value;
                    break;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.BusinessName).IsRequired().HasMaxLength(200);
            e.Property(x => x.ABN).IsRequired().HasMaxLength(20);
            e.Property(x => x.CreatedAt).IsRequired();

            e.HasQueryFilter(t =>
                _currentTenant.TenantId.HasValue && t.Id == _currentTenant.TenantId.Value);

            e.HasData(
                new Tenant
                {
                    Id = SeedData.Tenant1Id,
                    BusinessName = "Acme Cleaning Pty Ltd",
                    ABN = "11111111111",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Tenant
                {
                    Id = SeedData.Tenant2Id,
                    BusinessName = "Beta Maintenance Co",
                    ABN = "22222222222",
                    CreatedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                });
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(320);
            e.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
            e.Property(x => x.Role).IsRequired().HasMaxLength(20);
            e.HasIndex(x => new { x.TenantId, x.Email }).IsUnique();

            e.HasQueryFilter(u =>
                _currentTenant.TenantId.HasValue && u.TenantId == _currentTenant.TenantId.Value);

            e.HasOne(x => x.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasData(
                new User
                {
                    Id = SeedData.User1Id,
                    TenantId = SeedData.Tenant1Id,
                    Email = "admin@acme.demo",
                    PasswordHash = SeedData.DemoPasswordHash,
                    Role = "Admin"
                },
                new User
                {
                    Id = SeedData.User2Id,
                    TenantId = SeedData.Tenant2Id,
                    Email = "admin@beta.demo",
                    PasswordHash = SeedData.DemoPasswordHash,
                    Role = "Admin"
                });
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Phone).HasMaxLength(50);
            e.Property(x => x.Address).HasMaxLength(500);

            e.HasQueryFilter(c =>
                _currentTenant.TenantId.HasValue && c.TenantId == _currentTenant.TenantId.Value);

            e.HasOne(x => x.Tenant)
                .WithMany(t => t.Customers)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasData(
                new Customer
                {
                    Id = SeedData.Customer1Id,
                    TenantId = SeedData.Tenant1Id,
                    Name = "仅租户 A 可见的客户",
                    Phone = "0400000001",
                    Address = "Sydney NSW"
                },
                new Customer
                {
                    Id = SeedData.Customer2Id,
                    TenantId = SeedData.Tenant2Id,
                    Name = "仅租户 B 可见的客户",
                    Phone = "0400000002",
                    Address = "Melbourne VIC"
                });
        });
    }
}
