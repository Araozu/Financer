namespace Financer.Application.Dto;

public record TransactionResDto(
    Guid Id,
    DateTime Date,
    decimal Amount,
    string Description,
    Guid? AccountId,
    Guid? CategoryId,
    Guid CurrencyId
);
