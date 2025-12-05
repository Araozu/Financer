using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Commands;

public record DeleteCurrencyCommand(Guid Id) : IRequest<DeleteCurrencyResult>;

public record DeleteCurrencyResult(bool Success, string? ErrorMessage = null);

public class DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<DeleteCurrencyCommand, DeleteCurrencyResult>
{
    public async Task<DeleteCurrencyResult> Handle(
        DeleteCurrencyCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await currencyRepository.IsInUseAsync(request.Id))
        {
            return new DeleteCurrencyResult(
                false,
                "Currency cannot be deleted because it is in use by one or more transactions."
            );
        }

        var deleted = await currencyRepository.DeleteAsync(request.Id);
        return new DeleteCurrencyResult(deleted, deleted ? null : "Currency not found.");
    }
}
