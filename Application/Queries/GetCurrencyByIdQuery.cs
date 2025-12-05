using AutoMapper;
using Financer.Application.Dto;
using Financer.Domain.Repositories;
using MediatR;

namespace Financer.Application.Queries;

public record GetCurrencyByIdQuery(Guid Id) : IRequest<CurrencyResDto?>;

public class GetCurrencyByIdQueryHandler(ICurrencyRepository currencyRepository, IMapper mapper)
    : IRequestHandler<GetCurrencyByIdQuery, CurrencyResDto?>
{
    public async Task<CurrencyResDto?> Handle(
        GetCurrencyByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var currency = await currencyRepository.GetByIdAsync(request.Id);
        return currency == null ? null : mapper.Map<CurrencyResDto>(currency);
    }
}
