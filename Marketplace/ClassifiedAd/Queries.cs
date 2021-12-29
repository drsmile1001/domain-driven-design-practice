using static Marketplace.ClassifiedAd.ReadModels;

namespace Marketplace.ClassifiedAd;

public static class Queries
{
    public static ClassifiedAdDetails? Query(
        this IEnumerable<ClassifiedAdDetails> items,
        QueryModels.GetPublicClassifiedAd query)
        => items.FirstOrDefault(x => x.ClassifiedAdId == query.ClassifiedAdId);
}