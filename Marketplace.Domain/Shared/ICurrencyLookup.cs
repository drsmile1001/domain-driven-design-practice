namespace Marketplace.Domain.Shared;

public interface ICurrencyLookup
{
    Currency FindCurrency(string currencyCode);
}

public record Currency(string CurrencyCode, bool InUse, int DecimalPlaces)
{
    public readonly static Currency None = new("", false, 0);
}