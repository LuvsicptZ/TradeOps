using Microsoft.AspNetCore.Http;
using TradeOps.Application.Abstractions;

namespace TradeOps.Infrastructure.Middleware;

public sealed class TenantContextMiddleware(RequestDelegate next)
{
    private const string TenantIdClaim = "tenant_id";

    public async Task InvokeAsync(HttpContext context, ICurrentTenant currentTenant)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var raw = context.User.FindFirst(TenantIdClaim)?.Value;
            if (Guid.TryParse(raw, out var tenantId))
                currentTenant.TenantId = tenantId;
        }

        await next(context);
    }
}
