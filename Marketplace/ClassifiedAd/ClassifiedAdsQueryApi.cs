using System.Net;
using Marketplace.Projections;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.ClassifiedAd;

[Route("/ad")]
public class ClassifiedAdsQueryApi : ControllerBase
{
    private readonly List<ReadModels.ClassifiedAdDetails> _items;

    public ClassifiedAdsQueryApi(List<ReadModels.ClassifiedAdDetails> items)
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