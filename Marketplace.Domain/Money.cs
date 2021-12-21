using System.Runtime.Serialization;

namespace Marketplace.Domain;

public record Money
{
    private const string DefaultCurrency = "EUR";
    public static Money FromDecimal(decimal amount, string currency = DefaultCurrency)
        => new(amount, currency);

    public static Money FromString(string amount, string currency = DefaultCurrency)
        => new(decimal.Parse(amount), currency);

    protected Money(decimal amount, string currencyCode = DefaultCurrency)
    {
        if (decimal.Round(amount, 2) != amount)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot have more than two decimals");

        Amount = amount;
        CurrencyCode = currencyCode;
    }

    public decimal Amount { get; }
    public string CurrencyCode { get; }

    public Money Add(Money summand)
    {
        if (CurrencyCode != summand.CurrencyCode)
            throw new CurrencyMismatchException("Cannot sum amounts with different currencies");
        return new(Amount + summand.Amount, CurrencyCode);
    }
    public Money Subtract(Money subtrahend)
    {
        if (CurrencyCode != subtrahend.CurrencyCode)
            throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");
        return new(Amount - subtrahend.Amount, CurrencyCode);
    }

    public static Money operator +(Money a, Money b) => a.Add(b);
    public static Money operator -(Money a, Money b) => a.Subtract(b);
}

[Serializable]
internal class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string? message) : base(message)
    {
    }
}