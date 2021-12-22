using Marketplace.Framework;

namespace Marketplace.Domain;
public class ClassifiedAd : Entity
{
    public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
    {
        Id = id;
        OwnerId = ownerId;
        State = ClassifiedAdState.Inactive;
        EnsureValidState();
        Raise(new Events.ClassifiedAdCreated
        {
            Id = id,
            OwnerId = ownerId
        });
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

    public void SetTitle(ClassifiedAdTitle title)
    {
        Title = title;
        EnsureValidState();
        Raise(new Events.ClassifiedAdTitleChanged
        {
            Id = Id,
            Title = title
        });
    }

    public void UpdateText(ClassifiedAdText text)
    {
        Text = text;
        EnsureValidState();
        Raise(new Events.ClassifiedAdTextChanged
        {
            Id = Id,
            AdText = text
        });
    }

    public void UpdatePrice(Price price)
    {
        Price = price;
        EnsureValidState();
        Raise(new Events.ClassifiedAdPriceUpdated
        {
            Id = Id,
            Price = price.Amount,
            CurrencyCode = price.Currency.CurrencyCode
        });
    }

    public void RequestToPublish()
    {
        State = ClassifiedAdState.PendingReview;
        EnsureValidState();
        Raise(new Events.ClassifiedAdSentForReview
        {
            Id = Id
        });
    }

    protected void EnsureValidState()
    {
        var vaild = State switch
        {
            ClassifiedAdState.PendingReview =>
                Title != null
                && Text != null
                && Price?.Amount > 0,
            ClassifiedAdState.Active =>
                Title != null
                && Text != null
                && Price?.Amount > 0
                && ApprovedBy != null,
            _ => true
        };
        if (!vaild)
            throw new InvalidEntityStateException(this, $"Post-checks faild in state {State}");

    }

}

