namespace TradeOps.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }

    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public Tenant? Tenant { get; set; }
}