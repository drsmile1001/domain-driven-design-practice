﻿using Marketplace.Domain.Shared;
using Marketplace.Framework;

namespace Marketplace.Domain.UserProfile;

public class UserProfile : AggregateRoot
{
    public UserProfile(UserId id, FullName fullName, DisplayName displayName)
        => Apply(new Events.UserRegistered
        {
            UserId = id,
            FullName = fullName,
            DisplayName = displayName,
        });

    public UserId Id { get; private set; } = null!;

    public FullName FullName { get; private set; } = null!;

    public DisplayName DisplayName { get; private set; } = null!;

    public string PhotoUrl { get; private set; } = null!;

    public void UpdateFullName(FullName fullName)
        => Apply(new Events.UserFullNameUpdated
        {
            UserId = Id,
            FullName = fullName,
        });

    public void UpdateDisplayName(DisplayName displayName)
        => Apply(new Events.UserDisplayNameUpdated
        {
            UserId = Id,
            DisplayName = displayName,
        });

    public void UpdateProfilePhoto(Uri photoUrl)
        => Apply(new Events.ProfilePhotoUploaded
        {
            UserId = Id,
            PhotoUrl = photoUrl.ToString(),
        });

    protected override void When(object @event)
    {
        switch (@event)
        {
            case Events.UserRegistered e:
                Id = e.UserId;
                FullName = new FullName(e.FullName);
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case Events.UserFullNameUpdated e:
                FullName = new FullName(e.FullName);
                break;
            case Events.UserDisplayNameUpdated e:
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case Events.ProfilePhotoUploaded e:
                PhotoUrl = e.PhotoUrl;
                break;
            default:
                break;
        }
    }

    protected override void EnsureValidState()
    {
    }
}
