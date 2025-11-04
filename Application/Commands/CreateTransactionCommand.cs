using Financer.Domain.Entities;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Commands;

public record CreateTransactionCommand(
    DateTime Date,
    decimal Amount,
    string Description,
    Guid AccountId,
    Guid CategoryId
) : IRequest;

public class CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<CreateTransactionCommand>
{
    public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction(
            date: request.Date,
            amount: request.Amount,
            description: request.Description,
            accountId: request.AccountId,
            categoryId: request.CategoryId
        );

        await transactionRepository.AddAsync(transaction);
    }
}
