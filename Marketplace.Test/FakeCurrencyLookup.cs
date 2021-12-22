using System.Collections.Generic;
using Marketplace.Domain;
using System.Linq;

namespace Marketplace.Test;

public class FakeCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<CurrencyDetails>
    _currencies = new[]
    {
        new CurrencyDetails("EUR",true,2),
        new CurrencyDetails("USD",true,2),
        new CurrencyDetails("JPY",true,0),
        new CurrencyDetails("DEM",false,2),
    };

    public CurrencyDetails FindCurrency(string currencyCode)
    {
        var currency = _currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode);
        return currency ?? CurrencyDetails.None;
    }
}