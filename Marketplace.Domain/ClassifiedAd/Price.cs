using Marketplace.Domain.Shared;

namespace Marketplace.Domain.ClassifiedAd;

public record Price : Money
{
    /// <summary>
    /// 工廠方法建立值物件，將會檢查是否有效
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="currencyLookup"></param>
    /// <returns></returns>
    public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
    => new(amount, currency, currencyLookup);

    protected Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
    {
        if (Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(Amount), "Price cannot be negative");
    }

    /// <summary>
    /// 繞過檢查的建構式
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    internal Price(decimal amount, string currencyCode) : base(amount, currencyCode)
    {
    }
}