using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile;

public class DisplayName
{
    internal DisplayName(string value)
    {
        Value = value;
    }

    protected DisplayName()
    {
    }

    public string Value { get; init; } = null!;

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
