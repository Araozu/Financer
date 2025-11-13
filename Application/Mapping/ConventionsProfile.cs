using AutoMapper;
using Financer.Application.Dto;
using Financer.Domain.Entities;

namespace Financer.Application.Mapping;

public class ConventionsProfile : Profile
{
    public ConventionsProfile()
    {
        CreateMap<Transaction, TransactionResDto>();
    }
}
