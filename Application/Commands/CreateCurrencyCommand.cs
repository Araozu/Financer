using Financer.Domain.Entities;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Commands;

public record CreateCurrencyCommand(string Code, string Name, string Symbol) : IRequest<Guid>;

public class CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<CreateCurrencyCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCurrencyCommand request,
        CancellationToken cancellationToken
    )
    {
        var currency = new Currency(code: request.Code, name: request.Name, symbol: request.Symbol);

        await currencyRepository.AddAsync(currency);
        return currency.Id;
    }
}
