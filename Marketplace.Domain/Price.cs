namespace Marketplace.Domain;

public record Price : Money
{
    protected Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
    {
        if (Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(Amount), "Price cannot be negative");
    }
}