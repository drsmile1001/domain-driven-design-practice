using System.Text.RegularExpressions;

namespace Marketplace.Domain.ClassifiedAd;

public record ClassifiedAdTitle
{
    protected ClassifiedAdTitle()
    {
    }

    private ClassifiedAdTitle(string value)
    {
        if (value.Length > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");
        }

        Value = value;
    }

    public string Value { get; init; } = null!;

    public static implicit operator string(ClassifiedAdTitle title) => title.Value;

    public static implicit operator ClassifiedAdTitle(string title) => FromString(title);

    public static ClassifiedAdTitle FromString(string title)
        => new(title);

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "**")
            .Replace("</b>", "**");
        return new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty));
    }
}