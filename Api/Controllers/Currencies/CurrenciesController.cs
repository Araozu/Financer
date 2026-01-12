using System.ComponentModel.DataAnnotations;
using Financer.Application.Commands;
using Financer.Application.Queries;
using Financer.Domain.Utils;
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
        var result = await mediator.Send(new GetCurrencyByIdQuery(id));
        return result.Match<IActionResult>(
            currency => Ok(currency),
            notFound => NotFound(new ProblemDetails { Detail = notFound.Reason })
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyRequest request)
    {
        var command = new CreateCurrencyCommand(request.Code, request.Name, request.Symbol);
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
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(
            _ => NoContent(),
            notFound => NotFound(new ProblemDetails { Detail = notFound.Reason })
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCurrency(Guid id)
    {
        var result = await mediator.Send(new DeleteCurrencyCommand(id));
        return result.Match<IActionResult>(
            _ => NoContent(),
            notFound => NotFound(new ProblemDetails { Detail = notFound.Reason }),
            badRequest => Conflict(new ProblemDetails { Detail = badRequest.Reason })
        );
    }
}

public record CreateCurrencyRequest(
    [Required] [StringLength(10, MinimumLength = 1)] string Code,
    [Required] [StringLength(100, MinimumLength = 1)] string Name,
    [Required] [StringLength(10, MinimumLength = 1)] string Symbol
);

public record UpdateCurrencyRequest(
    [Required] [StringLength(10, MinimumLength = 1)] string Code,
    [Required] [StringLength(100, MinimumLength = 1)] string Name,
    [Required] [StringLength(10, MinimumLength = 1)] string Symbol
);
