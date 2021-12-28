using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;

namespace Marketplace.UserProfile;

public class UserProfileApplicationService : IApplicationService
{
    private readonly IAggregateStore _store;
    private readonly CheckTextForProfanity _checkText;

    public UserProfileApplicationService(
        CheckTextForProfanity checkTextForProfanity,
        IAggregateStore store)
    {
        _checkText = checkTextForProfanity;
        _store = store;
    }

    public async Task Handle(object command)
    {
        switch (command)
        {
            case Contracts.V1.RegisterUser cmd:
                await HandleCreate(cmd);
                break;
            case Contracts.V1.UpdateUserFullName cmd:
                await HandleUpdate(cmd.UserId, profile => profile.UpdateFullName(FullName.FromString(cmd.FullName)));
                break;
            case Contracts.V1.UpdateUserDisplayName cmd:
                await HandleUpdate(cmd.UserId, profile =>
                    profile.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, _checkText)));
                break;
            case Contracts.V1.UpdateUserProfilePhoto cmd:
                await HandleUpdate(cmd.UserId, profile =>
                    profile.UpdateProfilePhoto(new Uri(cmd.PhotoUrl)));
                break;
            default:
                throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown");
        }
    }

    private async Task HandleCreate(Contracts.V1.RegisterUser cmd)
    {
        if (await _store.Exits<Domain.UserProfile.UserProfile>(cmd.UserId.ToString()))
        {
            throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");
        }

        var userProfile = new Domain.UserProfile.UserProfile(
            cmd.UserId,
            FullName.FromString(cmd.FullName),
            DisplayName.FromString(cmd.DisplayName, _checkText));
        await _store.Save(userProfile, userProfile.Id.Value.ToString());
    }

    private async Task HandleUpdate(
        Guid userProfileId,
        Action<Domain.UserProfile.UserProfile> operation)
    {
        var classifiedAd = await _store.Load<Domain.UserProfile.UserProfile>(userProfileId.ToString());
        if (classifiedAd == null)
        {
            throw new InvalidOperationException($"Entity with id {userProfileId} cannot be found");
        }

        operation(classifiedAd);
        await _store.Save(classifiedAd, classifiedAd.Id.Value.ToString());
    }
}