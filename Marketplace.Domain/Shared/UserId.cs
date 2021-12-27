namespace Marketplace.Domain.Shared;

public record UserId
{
    public static readonly UserId Empty = new();

    public UserId(Guid value)
    {
        if (value == default)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;
    }

    protected UserId()
    {
    }

    public Guid Value { get; init; }

    public static implicit operator Guid(UserId id) => id.Value;

    public static implicit operator UserId(Guid v) => new(v);
}