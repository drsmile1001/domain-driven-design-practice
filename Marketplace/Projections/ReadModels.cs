namespace Marketplace.Projections;

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

    public class UserDetails
    {
        public Guid UserId { get; init; }

        public string DisplayName { get; set; } = null!;

        public string PhotoUrl { get; set; } = null!;
    }
}