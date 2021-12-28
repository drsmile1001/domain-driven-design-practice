namespace Marketplace.ClassifiedAd;

public static class QueryModels
{
    public class GetPublishedClassifiedAds
    {
        public int Page { get; init; }

        public int PageSize { get; init; }
    }

    public class GetOwnersClassifiedAd
    {
        public Guid OwnerId { get; init; }

        public int Page { get; init; }

        public int PageSize { get; init; }
    }

    public class GetPublicClassifiedAd
    {
        public Guid ClassifiedAdId { get; init; }
    }
}