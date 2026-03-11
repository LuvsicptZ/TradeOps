namespace TradeOps.Infrastructure.Persistence;

internal static class SeedData
{
    /// <summary>演示租户 A（登录：admin@acme.demo / Password1!）</summary>
    public static readonly Guid Tenant1Id = Guid.Parse("a1111111-1111-4111-8111-111111111101");

    /// <summary>演示租户 B（登录：admin@beta.demo / Password1!）</summary>
    public static readonly Guid Tenant2Id = Guid.Parse("b2222222-2222-4222-8222-222222222202");

    public static readonly Guid User1Id = Guid.Parse("c3333333-3333-4333-8333-333333333303");
    public static readonly Guid User2Id = Guid.Parse("d4444444-4444-4444-8444-444444444404");

    public static readonly Guid Customer1Id = Guid.Parse("e5555555-5555-4555-8555-555555555505");
    public static readonly Guid Customer2Id = Guid.Parse("f6666666-6666-4666-8666-666666666606");

    /// <summary>BCrypt hash for <c>Password1!</c> (work factor 11).</summary>
    public const string DemoPasswordHash = "$2a$11$QySpUPrhdV9dQ5HEFA9dv..Ou2rlJLQQYNRVUpF3Pc//YqpRvT/IW";
}
