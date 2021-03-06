using Microsoft.AspNetCore.Mvc;
using static Marketplace.ClassifiedAd.Commands;

namespace Marketplace.ClassifiedAd;

[ApiController]
[Route("/ad")]
public class ClassifiedAdsCommandsApi : ControllerBase
{
    private readonly ClassifiedAdsApplicationService _applicationService;

    public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(V1.Create request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("name")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.SetTitle request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("text")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.UpdateText request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("price")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.UpdatePrice request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("requestpublish")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.RequestToPublish request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("publish")]
    [HttpPut]
    public async Task<IActionResult> Put(V1.Publish request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }
}