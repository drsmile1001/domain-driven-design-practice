namespace Marketplace.Domain.UserProfile;

public static class Events
{
    public class UserRegistered
    {
        public Guid UserId { get; set; }

        public string FullName { get; init; } = null!;

        public string DisplayName { get; init; } = null!;
    }

    public class ProfilePhotoUploaded
    {
        public Guid UserId { get; set; }

        public string PhotoUrl { get; set; } = null!;
    }

    public class UserFullNameUpdated
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; } = null!;
    }

    public class UserDisplayNameUpdated
    {
        public Guid UserId { get; set; }

        public string DisplayName { get; set; } = null!;
    }
}