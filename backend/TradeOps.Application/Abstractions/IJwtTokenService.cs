using TradeOps.Domain.Entities;

namespace TradeOps.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateAccessToken(User user);
}
