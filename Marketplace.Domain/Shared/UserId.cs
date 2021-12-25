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

    private UserId()
    {
        Value = default;
    }

    public Guid Value { get; }

    public static implicit operator Guid(UserId id) => id.Value;

    public static implicit operator UserId(Guid v) => new(v);
}