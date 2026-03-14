using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeOps.Application.Abstractions;
using TradeOps.Domain.Entities;

namespace TradeOps.API.Controllers;

[ApiController]
[Route("users")]
[Authorize(Roles = "Admin")]
public sealed class UsersController : ControllerBase
{
    public sealed record CreateUserBody(string Email, string Password, string? Role);

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserBody body,
        [FromServices] ITradeOpsDbContext db,
        [FromServices] IPasswordHasher passwordHasher,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(body.Email) || string.IsNullOrWhiteSpace(body.Password))
            return BadRequest(new { message = "邮箱和密码不能为空。" });

        var role = string.IsNullOrWhiteSpace(body.Role) ? "Staff" : body.Role.Trim();
        if (role is not ("Admin" or "Staff"))
            return BadRequest(new { message = "角色只能是 Admin 或 Staff。" });

        var email = body.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email, cancellationToken))
            return Conflict(new { message = "该租户下已存在相同邮箱的用户。" });

        var user = new User
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.Empty,
            Email = email,
            PasswordHash = passwordHasher.Hash(body.Password),
            Role = role
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return Created($"/users/{user.Id}", new { user.Id, user.TenantId, user.Email, user.Role });
    }
}
