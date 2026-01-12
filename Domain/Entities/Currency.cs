namespace Financer.Domain.Entities;

public class Currency
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Symbol { get; private set; } = string.Empty;

    private Currency() { } // For EF

    public Currency(string code, string name, string symbol)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
    }

    public void Update(string code, string name, string symbol)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
    }
}
