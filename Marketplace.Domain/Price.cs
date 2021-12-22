namespace Marketplace.Domain;

public record Price : Money
{
    public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
    => new(amount, currency, currencyLookup);
    protected Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
    {
        if (Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(Amount), "Price cannot be negative");
    }
}