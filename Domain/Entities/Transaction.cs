namespace Financer.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Guid? AccountId { get; private set; }
    public Guid? CategoryId { get; private set; }

    // For event sourcing
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private Transaction() { } // For EF

    public Transaction(
        DateTime date,
        decimal amount,
        string description,
        Guid? accountId = null,
        Guid? categoryId = null
    )
    {
        Date = date;
        Amount = amount;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        AccountId = accountId;
        CategoryId = categoryId;
    }

    // For event sourcing, we could add domain events here, but keeping bare minimum
}
