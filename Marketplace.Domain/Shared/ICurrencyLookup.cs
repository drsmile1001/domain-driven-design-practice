namespace Marketplace.Domain.Shared;

public interface ICurrencyLookup
{
    Currency FindCurrency(string currencyCode);
}

public record Currency(string CurrencyCode, bool InUse, int DecimalPlaces)
{
    public static readonly Currency None = new(string.Empty, false, 0);
}