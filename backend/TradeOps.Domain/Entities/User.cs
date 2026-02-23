namespace TradeOps.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }

    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "Staff"; // Admin / Staff

    public Tenant? Tenant { get; set; }

}