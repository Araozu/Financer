using AutoMapper;
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
) : IRequest<IList<TransactionRes>>;

public class GetTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    IMapper mapper
) : IRequestHandler<GetTransactionsQuery, IList<TransactionRes>>
{
    public async Task<IList<TransactionRes>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await transactionRepository.GetAllAsync();
        var outList = transactions.Select(mapper.Map<TransactionRes>).ToList();
        return outList;
    }
}
