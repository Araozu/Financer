using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Commands;

public record DeleteCurrencyCommand(Guid Id) : IRequest;

public class DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<DeleteCurrencyCommand>
{
    public async Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        await currencyRepository.DeleteAsync(request.Id);
    }
}
