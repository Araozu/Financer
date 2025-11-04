using Financer.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financer.Api.Controllers.Transactions;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var query = new GetTransactionsQuery(DateTime.Now, 0L, "", Guid.Empty, Guid.Empty);
        var data = await mediator.Send(query);
        return Ok(data);
    }
}
