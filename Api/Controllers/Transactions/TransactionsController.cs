using Financer.Application.Commands;
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
        var data = await mediator.Send(new GetTransactionsQuery());
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
}
