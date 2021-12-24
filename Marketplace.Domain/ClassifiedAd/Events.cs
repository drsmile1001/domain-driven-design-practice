namespace Marketplace.Domain.ClassifiedAd;

public static class Events
{
    public class ClassifiedAdCreated
    {
        public Guid Id { get; init; }
        public Guid OwnerId { get; init; }
    }

    public class ClassifiedAdTitleChanged
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
    }

    public class ClassifiedAdTextChanged
    {
        public Guid Id { get; init; }
        public string AdText { get; init; } = null!;
    }

    public class ClassifiedAdPriceUpdated
    {
        public Guid Id { get; init; }
        public decimal Price { get; init; }
        public string CurrencyCode { get; init; } = null!;
    }

    public class ClassifiedAdSentForReview
    {
        public Guid Id { get; init; }
    }

    public class PictureAddedToAClassifiedAd
    {
        public Guid PictureId { get; init; }
        public ClassifiedAdId ClassifiedAId { get; init; } = null!;
        public string Uri { get; init; } = null!;
        public int Height { get; init; }
        public int Width { get; init; }
        public int Order { get; init; }
    }

    public class ClassifiedAdPictureResized
    {
        public Guid PictureId { get; init; }
        public int Height { get; init; }
        public int Width { get; init; }
    }
}