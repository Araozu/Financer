using AutoMapper;
using Financer.Application.Dto;
using Financer.Domain.Repositories;
using Financer.Domain.Utils;
using MediatR;
using OneOf;

namespace Financer.Application.Queries;

public record GetCurrencyByIdQuery(Guid Id) : IRequest<OneOf<CurrencyResDto, NotFound>>;

public class GetCurrencyByIdQueryHandler(ICurrencyRepository currencyRepository, IMapper mapper)
    : IRequestHandler<GetCurrencyByIdQuery, OneOf<CurrencyResDto, NotFound>>
{
    public async Task<OneOf<CurrencyResDto, NotFound>> Handle(
        GetCurrencyByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var currency = await currencyRepository.GetByIdAsync(request.Id);
        if (currency == null)
            return new NotFound("Currency not found.");

        return mapper.Map<CurrencyResDto>(currency);
    }
}
