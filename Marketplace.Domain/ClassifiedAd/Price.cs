using Marketplace.Domain.Shared;

namespace Marketplace.Domain.ClassifiedAd;

public record Price : Money
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> class.
    /// </summary>
    /// <returns>繞過檢查的建構式.</returns>
    internal Price(decimal amount, string currencyCode)
        : base(amount, currencyCode)
    {
    }

    protected Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
        : base(amount, currencyCode, currencyLookup)
    {
        if (Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Amount), "Price cannot be negative");
        }
    }

    protected Price()
    {
    }

    /// <summary>
    /// 工廠方法建立值物件，將會檢查是否有效.
    /// </summary>
    /// <returns>Price.</returns>
    public static new Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
    => new(amount, currency, currencyLookup);
}