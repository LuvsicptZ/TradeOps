using Microsoft.EntityFrameworkCore;
using TradeOps.Application.Abstractions;
using TradeOps.Application.Auth;

namespace TradeOps.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly ITradeOpsDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        ITradeOpsDbContext db,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(request.Password))
        {
            return new LoginResult(false, null, "邮箱和密码不能为空。");
        }

        var candidates = await _db.Users
            .IgnoreQueryFilters()
            .Where(u => u.Email == email)
            .ToListAsync(cancellationToken);

        foreach (var user in candidates)
        {
            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                continue;

            if (user.Role is not ("Admin" or "Staff"))
                continue;

            var token = _jwtTokenService.CreateAccessToken(user);
            return new LoginResult(true, token, null);
        }

        return new LoginResult(false, null, "邮箱或密码错误。");
    }
}
