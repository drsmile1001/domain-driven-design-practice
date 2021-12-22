namespace Marketplace.Domain;

public interface ICurrencyLookup
{
    CurrencyDetails FindCurrency(string currencyCode);
}

public record CurrencyDetails(string CurrencyCode, bool InUse, int DecimalPlaces)
{
    public readonly static CurrencyDetails None = new("", false, 0);
}