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

    public static implicit operator Guid(ClassifiedAdId id) => id.Value;
    public static implicit operator ClassifiedAdId(Guid v) => new(v);
}