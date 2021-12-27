using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.ClassifiedAd;

[Route("/ad")]
public class ClassifiedAdsQueryApi : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
    {
        return Ok();
    }

    [HttpGet]
    [Route("myads")]
    public async Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
    {
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request)
    {
        return Ok();
    }
}