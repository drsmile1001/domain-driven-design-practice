using Marketplace.Domain.Shared;

namespace Marketplace;

public class FixedCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<Currency> _currencies =
        new[]
        {
                new Currency("EUR", true, 2),
                new Currency("USD", true, 2),
        };

    public Currency FindCurrency(string currencyCode)
    {
        var currency = _currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
        return currency ?? Currency.None;
    }
}
