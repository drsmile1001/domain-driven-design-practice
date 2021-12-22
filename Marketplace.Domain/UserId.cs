namespace Marketplace.Domain;

public record UserId
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        if (value == default)
            throw new ArgumentNullException(nameof(value));
        Value = value;
    }

    public static implicit operator Guid(UserId id) => id.Value;
    public static explicit operator UserId(Guid v) => new(v);
}