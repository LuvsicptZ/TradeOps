namespace TradeOps.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public required string BusinessName { get; set; }
    public required string ABN { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}