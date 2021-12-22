using Marketplace.Framework;

namespace Marketplace.Domain;
public class ClassifiedAd : Entity
{
    public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
    //TODO: Nullable 後，建構式也使用與其他領域事件相同做法會導致語義不是很正確
    => Apply(new Events.ClassifiedAdCreated
    {
        Id = id,
        OwnerId = ownerId
    });

    public enum ClassifiedAdState
    {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold
    }

    public ClassifiedAdId Id { get; private set; }
    public UserId OwnerId { get; private set; }
    public ClassifiedAdState State { get; private set; }
    public ClassifiedAdTitle? Title { get; private set; }
    public ClassifiedAdText? Text { get; private set; }
    public Price? Price { get; private set; }
    public UserId? ApprovedBy { get; private set; }

    public void SetTitle(ClassifiedAdTitle title)
    => Apply(new Events.ClassifiedAdTitleChanged
    {
        Id = Id,
        Title = title
    });

    public void UpdateText(ClassifiedAdText text)
    => Apply(new Events.ClassifiedAdTextChanged
    {
        Id = Id,
        AdText = text
    });

    public void UpdatePrice(Price price)
    => Apply(new Events.ClassifiedAdPriceUpdated
    {
        Id = Id,
        Price = price.Amount,
        CurrencyCode = price.CurrencyCode
    });

    public void RequestToPublish()
    => Apply(new Events.ClassifiedAdSentForReview
    {
        Id = Id
    });

    protected override void When(object @event)
    {
        switch (@event)
        {
            case Events.ClassifiedAdCreated e:
                Id = e.Id;
                OwnerId = e.OwnerId;
                State = ClassifiedAdState.Inactive;
                break;
            case Events.ClassifiedAdTitleChanged e:
                Title = ClassifiedAdTitle.FromString(e.Title);
                break;
            case Events.ClassifiedAdTextChanged e:
                Text = new ClassifiedAdText(e.AdText);
                break;
            case Events.ClassifiedAdPriceUpdated e:
                //TODO: 釐清在套用事件時，建立值物件的做法
                //第5章說明事件為了在未來改變規則後還能載入，必須跳過檢查
                //所以事件的屬性不使用值物件
                //但截至第5章，檢查值物件是否合法只發生在值物件的工廠方法
                //應該與事件是否使用值物件傳遞資料無關？
                Price = new Price(e.Price, e.CurrencyCode);
                break;
            case Events.ClassifiedAdSentForReview:
                State = ClassifiedAdState.PendingReview;
                break;
            default:
                break;
        }

    }

    protected override void EnsureValidState()
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

