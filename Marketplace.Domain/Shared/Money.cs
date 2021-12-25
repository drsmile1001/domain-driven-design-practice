namespace Marketplace.Domain.Shared;

public record Money
{
    protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");
        }

        var currency = currencyLookup.FindCurrency(currencyCode);
        if (!currency.InUse)
        {
            throw new ArgumentException($"Currency {currencyCode} is not valid");
        }

        if (decimal.Round(amount, currency.DecimalPlaces) != amount)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Amount in {currencyCode} can not have more than {currency.DecimalPlaces} decimals");
        }

        Amount = amount;
        CurrencyCode = currencyCode;
    }

    protected Money(decimal amount, string currencyCode)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
    }

    public decimal Amount { get; }

    public string CurrencyCode { get; }

    public static Money operator +(Money a, Money b) => a.Add(b);

    public static Money operator -(Money a, Money b) => a.Subtract(b);

    public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
        => new(amount, currency, currencyLookup);

    public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup)
        => new(decimal.Parse(amount), currency, currencyLookup);

    public Money Add(Money summand)
    {
        if (CurrencyCode != summand.CurrencyCode)
        {
            throw new CurrencyMismatchException("Cannot sum amounts with different currencies");
        }

        return new(Amount + summand.Amount, CurrencyCode);
    }

    public Money Subtract(Money subtrahend)
    {
        if (CurrencyCode != subtrahend.CurrencyCode)
        {
            throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");
        }

        return new(Amount - subtrahend.Amount, CurrencyCode);
    }

    public override string ToString()
        => $"{CurrencyCode} {Amount}";
}

[Serializable]
public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string? message)
        : base(message)
    {
    }
}