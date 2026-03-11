using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TradeOps.Infrastructure.Security;

namespace TradeOps.Infrastructure.Persistence;

/// <summary>供 <c>dotnet ef migrations</c> 在设计时创建 DbContext（无 HTTP / DI 租户上下文）。</summary>
public sealed class TradeOpsDbContextFactory : IDesignTimeDbContextFactory<TradeOpsDbContext>
{
    public TradeOpsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TradeOpsDbContext>();
        var cs = Environment.GetEnvironmentVariable("TRADEOPS_CONNECTION_STRING")
                 ?? "Host=127.0.0.1;Port=5432;Database=tradeops;Username=postgres;Password=postgres";
        optionsBuilder.UseNpgsql(cs);
        return new TradeOpsDbContext(optionsBuilder.Options, new CurrentTenant());
    }
}
