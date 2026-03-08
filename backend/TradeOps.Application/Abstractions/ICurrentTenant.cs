namespace TradeOps.Application.Abstractions;

public interface ICurrentTenant
{
    Guid? TenantId { get; set; }
}
