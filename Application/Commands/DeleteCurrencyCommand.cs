using Financer.Domain.Repositories;
using Financer.Domain.Utils;
using MediatR;
using OneOf;

namespace Financer.Application.Commands;

public record DeleteCurrencyCommand(Guid Id) : IRequest<OneOf<Success, NotFound, BadRequest>>;

public class DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<DeleteCurrencyCommand, OneOf<Success, NotFound, BadRequest>>
{
    public async Task<OneOf<Success, NotFound, BadRequest>> Handle(
        DeleteCurrencyCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await currencyRepository.IsInUseAsync(request.Id))
        {
            return new BadRequest(
                "Currency cannot be deleted because it is in use by one or more transactions."
            );
        }

        var deleted = await currencyRepository.DeleteAsync(request.Id);
        if (!deleted)
            return new NotFound("Currency not found.");

        return new Success();
    }
}
