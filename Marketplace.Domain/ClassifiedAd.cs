namespace Marketplace.Domain;
public class ClassifiedAd
{
    public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
    {
        Id = id;
        OwnerId = ownerId;
        State = ClassifiedAdState.Inactive;
    }

    public enum ClassifiedAdState
    {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold
    }

    public ClassifiedAdId Id { get; }
    public UserId OwnerId { get; }
    public ClassifiedAdState State { get; private set; }
    public ClassifiedAdTitle? Title { get; private set; }
    public ClassifiedAdText? Text { get; private set; }
    public Price? Price { get; private set; }
    public UserId? ApprovedBy { get; private set; }

    public void SetTitle(ClassifiedAdTitle title) => Title = title;
    public void UpdateText(ClassifiedAdText text) => Text = text;
    public void UpdatePrice(Price price) => Price = price;

    public void RequestToPublish()
    {
        if (Title == null)
            throw new InvalidEntityStateException(this, "Title cannot be empty");
        if (Text == null)
            throw new InvalidEntityStateException(this, "Text cannot be empty");
        if (Price?.Amount == 0)
            throw new InvalidEntityStateException(this, "Price cannot be zero");
        State = ClassifiedAdState.PendingReview;
    }

}

