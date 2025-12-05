using AutoMapper;
using Financer.Application.Dto;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Queries;

public record GetCurrenciesQuery : IRequest<IList<CurrencyResDto>>;

public class GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository, IMapper mapper)
    : IRequestHandler<GetCurrenciesQuery, IList<CurrencyResDto>>
{
    public async Task<IList<CurrencyResDto>> Handle(
        GetCurrenciesQuery request,
        CancellationToken cancellationToken
    )
    {
        var currencies = await currencyRepository.GetAllAsync();
        var outList = currencies.Select(mapper.Map<CurrencyResDto>).ToList();
        return outList;
    }
}
