namespace TradeOps.Application.Auth;

public sealed record LoginResult(bool Success, string? AccessToken, string? Error);
