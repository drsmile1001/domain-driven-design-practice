namespace Marketplace.Domain.Shared;

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
    public static implicit operator UserId(Guid v) => new(v);
}