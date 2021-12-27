namespace Marketplace.Domain.UserProfile;

public record FullName
{
    internal FullName(string fullName) => Value = fullName;

    /// <summary>
    /// Initializes a new instance of the <see cref="FullName"/> class.
    /// 反序列化用建構式.
    /// </summary>
    protected FullName()
    {
    }

    public string Value { get; init; } = null!;

    public static implicit operator string(FullName value) => value.Value;

    public static FullName FromString(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentNullException(nameof(fullName));
        }

        return new FullName(fullName);
    }
}