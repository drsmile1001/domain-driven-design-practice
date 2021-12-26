using Marketplace.Domain.Shared;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;

namespace Marketplace.UserProfile;

public class UserProfileRepository : RavenDbRepository<Domain.UserProfile.UserProfile, UserId>,
    Domain.UserProfile.IUserProfileRepositoy
{
    public UserProfileRepository(
        IAsyncDocumentSession session)
        : base(session, id => $"UserProfile/{id}")
    {
    }
}
