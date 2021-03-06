using Marketplace.Domain.Shared;
using Marketplace.Framework;
using static Marketplace.Domain.Shared.DomainExceptions;

namespace Marketplace.Domain.ClassifiedAd;
public class ClassifiedAd : AggregateRoot
{
    public ClassifiedAd(ClassifiedAdId id, UserId ownerId) => Apply(new Events.ClassifiedAdCreated
    {
        Id = id,
        OwnerId = ownerId,
    });

    protected ClassifiedAd()
    {
    }

    public enum ClassifiedAdState
    {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold,
    }

    public ClassifiedAdId Id { get; private set; } = null!;

    public UserId OwnerId { get; private set; } = null!;

    public ClassifiedAdState State { get; private set; }

    public ClassifiedAdTitle? Title { get; private set; }

    public ClassifiedAdText? Text { get; private set; }

    public Price? Price { get; private set; }

    public UserId? ApprovedBy { get; private set; }

    public List<Picture> Pictures { get; private set; } = new List<Picture>();

    private string DbId
    {
        get => $"ClassifiedAd/{Id.Value}";
        set { }
    }

    private Picture? FirstPicture
        => Pictures.OrderBy(x => x.Order).FirstOrDefault();

    public void SetTitle(ClassifiedAdTitle title)
    => Apply(new Events.ClassifiedAdTitleChanged
    {
        Id = Id,
        Title = title,
    });

    public void UpdateText(ClassifiedAdText text)
    => Apply(new Events.ClassifiedAdTextChanged
    {
        Id = Id,
        AdText = text,
    });

    public void UpdatePrice(Price price)
    => Apply(new Events.ClassifiedAdPriceUpdated
    {
        Id = Id,
        Price = price.Amount,
        CurrencyCode = price.CurrencyCode,
    });

    public void RequestToPublish()
    => Apply(new Events.ClassifiedAdSentForReview
    {
        Id = Id,
    });

    public void Publish(UserId userId)
        => Apply(new Events.ClassifiedAdPublished
        {
            Id = Id,
            ApprovedBy = userId,
        });

    public void AddPicture(Uri pictureUri, PictureSize size)
    => Apply(new Events.PictureAddedToAClassifiedAd
    {
        PictureId = Guid.NewGuid(),
        ClassifiedAId = Id,
        Uri = pictureUri.ToString(),
        Height = size.Height,
        Width = size.Width,
        Order = Pictures.Max(x => x.Order) + 1,
    });

    public void ResizePicture(PictureId pictureId, PictureSize newSize)
    {
        var picture = FindPicture(pictureId);
        if (picture == null)
        {
            throw new InvalidOperationException(
                "Cannot resize a picture that I don't have");
        }

        picture.Resize(newSize);
    }

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
                Title = e.Title;
                break;
            case Events.ClassifiedAdTextChanged e:
                Text = e.AdText;
                break;
            case Events.ClassifiedAdPriceUpdated e:
                // TODO: 釐清在套用事件時，建立值物件的做法
                // 第5章說明事件為了在未來改變規則後還能載入，必須跳過檢查
                // 所以事件的屬性不使用值物件
                // 但截至第5章，檢查值物件是否合法只發生在值物件的工廠方法
                // 應該與事件是否使用值物件傳遞資料無關？
                Price = new Price(e.Price, e.CurrencyCode);
                break;
            case Events.ClassifiedAdSentForReview:
                State = ClassifiedAdState.PendingReview;
                break;
            case Events.ClassifiedAdPublished e:
                ApprovedBy = e.ApprovedBy;
                State = ClassifiedAdState.Active;
                break;
            case Events.PictureAddedToAClassifiedAd e:
                var picture = new Picture(Apply);
                ApplyToEntity(picture, e);
                Pictures.Add(picture);
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
            _ => true,
        };
        if (!vaild)
        {
            throw new InvalidEntityStateException(this, $"Post-checks faild in state {State}");
        }
    }

    private Picture? FindPicture(PictureId id)
        => Pictures.FirstOrDefault(x => x.Id == id);
}
