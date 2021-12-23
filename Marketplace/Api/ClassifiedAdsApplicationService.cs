using Marketplace.Framework;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api;
public class ClassifiedAdsApplicationService : IApplicationService
{
    public async Task Handle(object command)
    {
        switch (command)
        {
            case V1.Create cmd:
                break;
            default:
                throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown");
        }
    }
}