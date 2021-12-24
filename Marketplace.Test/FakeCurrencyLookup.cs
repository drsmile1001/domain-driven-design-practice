using System.Collections.Generic;
using Marketplace.Domain;
using System.Linq;
using Marketplace.Domain.Shared;

namespace Marketplace.Test;

public class FakeCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<Currency>
    _currencies = new[]
    {
        new Currency("EUR",true,2),
        new Currency("USD",true,2),
        new Currency("JPY",true,0),
        new Currency("DEM",false,2),
    };

    public Currency FindCurrency(string currencyCode)
    {
        var currency = _currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode);
        return currency ?? Currency.None;
    }
}