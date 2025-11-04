using Financer.Application.Dto;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Queries;

public record GetTransactionsQuery(
    DateTime Date,
    decimal Amount,
    string Description,
    Guid AccountId,
    Guid CategoryId
) : IRequest<TransactionRes>;

public class GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<GetTransactionsQuery, TransactionRes>
{
    public async Task<TransactionRes> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await transactionRepository.GetAllAsync();
        return new TransactionRes(Guid.NewGuid());
    }
}
