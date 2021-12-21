namespace Marketplace.Domain;

public record ClassifiedAdId
{
    public Guid Value { get; }

    public ClassifiedAdId(Guid value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(value));
        Value = value;
    }
}