using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile;

public interface IUserProfileRepositoy
{
    Task<UserProfile> Load(UserId id);

    Task Add(UserProfile entitiy);

    Task<bool> Exists(UserId id);
}
