namespace Marketplace.Domain.UserProfile;

public static class Events
{
    public class UserRegistered
    {
        public Guid UserId { get; init; }

        public string FullName { get; init; } = null!;

        public string DisplayName { get; init; } = null!;
    }

    public class ProfilePhotoUploaded
    {
        public Guid UserId { get; init; }

        public string PhotoUrl { get; init; } = null!;
    }

    public class UserFullNameUpdated
    {
        public Guid UserId { get; init; }

        public string FullName { get; init; } = null!;
    }

    public class UserDisplayNameUpdated
    {
        public Guid UserId { get; init; }

        public string DisplayName { get; init; } = null!;
    }
}