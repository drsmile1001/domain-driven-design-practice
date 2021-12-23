namespace Marketplace.Domain;

public class Picture
{
    public PictureId Id { get; init; } = null!;
    internal PictureSize Size { get; set; } = null!;
    internal Uri Location { get; set; } = null!;
    internal int Order { get; set; }

    protected void When(object @event)
    {
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