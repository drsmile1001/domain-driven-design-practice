using System.Net;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace Marketplace.ClassifiedAd;

[Route("/ad")]
public class ClassifiedAdsQueryApi : ControllerBase
{
    private readonly IAsyncDocumentSession _session;

    public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
    {
        _session = session;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
    {
        var ads = await _session.Query(request);
        return Ok(ads);
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