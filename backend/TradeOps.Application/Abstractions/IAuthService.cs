using TradeOps.Application.Auth;

namespace TradeOps.Application.Abstractions;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
