using TradeOps.Application.Abstractions;

namespace TradeOps.Infrastructure.Security;

public sealed class CurrentTenant : ICurrentTenant
{
    public Guid? TenantId { get; set; }
}
