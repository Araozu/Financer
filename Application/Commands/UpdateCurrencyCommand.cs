using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Commands;

public record UpdateCurrencyCommand(Guid Id, string Code, string Name, string Symbol)
    : IRequest<bool>;

public class UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<UpdateCurrencyCommand, bool>
{
    public async Task<bool> Handle(
        UpdateCurrencyCommand request,
        CancellationToken cancellationToken
    )
    {
        var currency = await currencyRepository.GetByIdAsync(request.Id);
        if (currency == null)
            return false;

        currency.Update(request.Code, request.Name, request.Symbol);
        await currencyRepository.UpdateAsync(currency);
        return true;
    }
}
