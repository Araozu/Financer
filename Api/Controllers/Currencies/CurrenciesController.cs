using Financer.Application.Commands;
using Financer.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financer.Api.Controllers.Currencies;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        var data = await mediator.Send(new GetCurrenciesQuery());
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCurrency(Guid id)
    {
        var data = await mediator.Send(new GetCurrencyByIdQuery(id));
        if (data == null)
            return NotFound();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCurrency), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCurrency(
        Guid id,
        [FromBody] UpdateCurrencyRequest request
    )
    {
        var command = new UpdateCurrencyCommand(id, request.Code, request.Name, request.Symbol);
        var success = await mediator.Send(command);
        if (!success)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCurrency(Guid id)
    {
        await mediator.Send(new DeleteCurrencyCommand(id));
        return NoContent();
    }
}

public record UpdateCurrencyRequest(string Code, string Name, string Symbol);
