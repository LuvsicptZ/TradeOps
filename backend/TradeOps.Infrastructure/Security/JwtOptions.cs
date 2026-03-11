namespace TradeOps.Infrastructure.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "TradeOps";
    public string Audience { get; set; } = "TradeOps";
    public int ExpirationMinutes { get; set; } = 60;
}
