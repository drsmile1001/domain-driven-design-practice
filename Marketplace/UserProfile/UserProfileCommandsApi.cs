using Microsoft.AspNetCore.Mvc;
using static Marketplace.UserProfile.Contracts;

namespace Marketplace.UserProfile;

[ApiController]
[Route("/profile")]
public class UserProfileCommandsApi : ControllerBase
{
    private readonly UserProfileApplicationService _applicationService;

    public UserProfileCommandsApi(UserProfileApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(V1.RegisterUser request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("fullname")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.UpdateUserFullName request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("displayname")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.UpdateUserDisplayName request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("photo")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.UpdateUserProfilePhoto request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }
}
