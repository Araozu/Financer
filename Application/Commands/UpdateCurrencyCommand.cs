using Financer.Domain.Repositories;
using Financer.Domain.Utils;
using MediatR;
using OneOf;

namespace Financer.Application.Commands;

public record UpdateCurrencyCommand(Guid Id, string Code, string Name, string Symbol)
    : IRequest<OneOf<Success, NotFound>>;

public class UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<UpdateCurrencyCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(
        UpdateCurrencyCommand request,
        CancellationToken cancellationToken
    )
    {
        var currency = await currencyRepository.GetByIdAsync(request.Id);
        if (currency == null)
            return new NotFound("Currency not found.");

        currency.Update(request.Code, request.Name, request.Symbol);
        await currencyRepository.UpdateAsync(currency);
        return new Success();
    }
}
