using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile;

public class DisplayName
{
    public static readonly DisplayName Empty = new();

    internal DisplayName(string value)
    {
        Value = value;
    }

    protected DisplayName()
    {
        Value = string.Empty;
    }

    public string Value { get; }

    public static implicit operator string(DisplayName name) => name.Value;

    public static DisplayName FromString(string displayName, CheckTextForProfanity hasProfanity)
    {
        if (string.IsNullOrEmpty(displayName))
        {
            throw new ArgumentNullException(nameof(displayName));
        }

        if (hasProfanity(displayName))
        {
            throw new DomainExceptions.ProfanityFound(displayName);
        }

        return new DisplayName(displayName);
    }
}
