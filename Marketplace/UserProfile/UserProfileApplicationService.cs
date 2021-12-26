using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.Domain.Shared;
using System;

namespace Marketplace.UserProfile;

public class UserProfileApplicationService : IApplicationService
{
    private readonly IUserProfileRepositoy _repositoy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CheckTextForProfanity _checkText;

    public UserProfileApplicationService(
        IUserProfileRepositoy repositoy,
        IUnitOfWork unitOfWork,
        CheckTextForProfanity checkTextForProfanity)
    {
        _repositoy = repositoy;
        _unitOfWork = unitOfWork;
        _checkText = checkTextForProfanity;
    }

    public async Task Handle(object command)
    {
        switch (command)
        {
            case Contracts.V1.RegisterUser cmd:
                if (await _repositoy.Exists(cmd.UserId))
                {
                    throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");
                }

                var userProfile = new Domain.UserProfile.UserProfile(
                    cmd.UserId,
                    FullName.FromString(cmd.FullName),
                    DisplayName.FromString(cmd.DisplayName, _checkText));
                await _repositoy.Add(userProfile);
                await _unitOfWork.Commit();
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

    private async Task HandleUpdate(
        Guid userProfileId,
        Action<Domain.UserProfile.UserProfile> operation)
    {
        var classifiedAd = await _repositoy.Load(userProfileId);
        if (classifiedAd == null)
        {
            throw new InvalidOperationException($"Entity with id {userProfileId} cannot be found");
        }

        operation(classifiedAd);

        await _unitOfWork.Commit();
    }
}