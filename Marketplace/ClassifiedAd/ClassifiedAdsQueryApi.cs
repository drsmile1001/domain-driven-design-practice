using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.ClassifiedAd;

[Route("/ad")]
public class ClassifiedAdsQueryApi : ControllerBase
{
    private readonly IList<ReadModels.ClassifiedAdDetails> _items;

    public ClassifiedAdsQueryApi(IList<ReadModels.ClassifiedAdDetails> items)
    {
        _items = items;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public IActionResult Get(QueryModels.GetPublicClassifiedAd request)
    {
        var found = _items.Query(request);
        return Ok(found);
    }
}