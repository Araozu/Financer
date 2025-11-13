using AutoMapper;
using Financer.Application.Dto;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Queries;

public record GetTransactionsQuery : IRequest<IList<TransactionResDto>>;

public class GetTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    IMapper mapper
) : IRequestHandler<GetTransactionsQuery, IList<TransactionResDto>>
{
    public async Task<IList<TransactionResDto>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await transactionRepository.GetAllAsync();
        var outList = transactions.Select(mapper.Map<TransactionResDto>).ToList();
        return outList;
    }
}
