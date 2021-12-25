namespace Marketplace.Domain.ClassifiedAd;

public record ClassifiedAdId
{
    public ClassifiedAdId(Guid value)
    {
        if (value == default)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(ClassifiedAdId id) => id.Value;

    public static implicit operator ClassifiedAdId(Guid v) => new(v);
}