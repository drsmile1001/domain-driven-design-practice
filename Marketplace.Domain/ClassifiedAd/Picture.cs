using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd;

public class Picture : Entity
{
    public Picture(Action<object> applier)
        : base(applier)
    {
    }

    public PictureId? Id { get; set; }

    internal PictureSize? Size { get; set; }

    internal Uri? Location { get; set; }

    internal int Order { get; set; }

    internal void Resize(PictureSize newSize)
        => Apply(new Events.ClassifiedAdPictureResized
        {
            PictureId = Id.Value,
            Height = newSize.Height,
            Width = newSize.Width,
        });

    protected override void When(object @event)
    {
        switch (@event)
        {
            case Events.PictureAddedToAClassifiedAd e:
                Id = new PictureId(e.PictureId);
                Location = new Uri(e.Uri);
                Size = new PictureSize
                {
                    Height = e.Height,
                    Width = e.Width,
                };
                Order = e.Order;
                break;
            case Events.ClassifiedAdPictureResized e:
                Size = new PictureSize
                {
                    Height = e.Height,
                    Width = e.Width,
                };
                break;
            default:
                break;
        }
    }
}

public record PictureSize
{
    public PictureSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    internal PictureSize()
    {
    }

    public int Width { get; init; }

    public int Height { get; init; }
}

public record PictureId(Guid Value);