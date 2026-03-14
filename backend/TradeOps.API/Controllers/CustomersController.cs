using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeOps.Application.Abstractions;

namespace TradeOps.API.Controllers;

[ApiController]
[Route("customers")]
[Authorize]
public sealed class CustomersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] ITradeOpsDbContext db,
        CancellationToken cancellationToken)
    {
        var list = await db.Customers
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.TenantId, c.Name, c.Phone, c.Address })
            .ToListAsync(cancellationToken);

        return Ok(list);
    }
}
