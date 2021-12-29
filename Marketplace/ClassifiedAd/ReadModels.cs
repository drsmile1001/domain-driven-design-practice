namespace Marketplace.ClassifiedAd;

public static class ReadModels
{
    public class ClassifiedAdDetails
    {
        public Guid ClassifiedAdId { get; init; }

        public Guid SellerId { get; init; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string SellersDisplayName { get; set; } = null!;

        public string[] PhotoUrls { get; set; } = null!;
    }

    public class ClassifiedAdListItem
    {
        public Guid ClassifiedAdId { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public string PhotoUrl { get; set; } = null!;
    }
}