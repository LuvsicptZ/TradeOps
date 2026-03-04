using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TradeOps.Domain.Entities;
using TradeOps.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TradeOpsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("TradeOps API is running."));

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "TradeOps.API",
    utcTime = DateTime.UtcNow
}));

app.MapPost("/users", async (CreateUserRequest request, TradeOpsDbContext db) =>
{
    if (request.TenantId == Guid.Empty)
    {
        return Results.BadRequest(new { message = "TenantId is required." });
    }

    if (string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.BadRequest(new { message = "Email is required." });
    }

    if (string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new { message = "Password is required." });
    }

    var tenantExists = await db.Tenants.AnyAsync(t => t.Id == request.TenantId);
    if (!tenantExists)
    {
        return Results.BadRequest(new { message = "Tenant not found." });
    }

    var normalizedEmail = request.Email.Trim().ToLowerInvariant();
    var userExists = await db.Users.AnyAsync(u =>
        u.TenantId == request.TenantId && u.Email == normalizedEmail);

    if (userExists)
    {
        return Results.Conflict(new { message = "Email already exists in this tenant." });
    }

    var user = new User
    {
        Id = Guid.NewGuid(),
        TenantId = request.TenantId,
        Email = normalizedEmail,
        PasswordHash = ComputeSha256(request.Password),
        Role = string.IsNullOrWhiteSpace(request.Role) ? "Staff" : request.Role.Trim()
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", new UserResponse(
        user.Id,
        user.TenantId,
        user.Email,
        user.Role
    ));
});

app.Run();

static string ComputeSha256(string value)
{
    var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
    return Convert.ToHexString(bytes);
}

public sealed record CreateUserRequest(
    Guid TenantId,
    string Email,
    string Password,
    string? Role
);

public sealed record UserResponse(
    Guid Id,
    Guid TenantId,
    string Email,
    string Role
);
