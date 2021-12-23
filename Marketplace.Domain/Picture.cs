using Marketplace.Framework;

namespace Marketplace.Domain;

public class Picture : Entity
{
    public Picture(Action<object> applier) : base(applier)
    {
    }

    public PictureId? Id { get; set; }
    internal PictureSize? Size { get; set; }
    internal Uri? Location { get; set; }
    internal int Order { get; set; }

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
                    Width = e.Width
                };
                Order = e.Order;
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

    public int Width { get; internal init; }

    public int Height { get; internal init; }
}

public record PictureId(Guid Value);