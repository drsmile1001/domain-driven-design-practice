namespace Marketplace.Domain.ClassifiedAd;

public record ClassifiedAdText(string Value)
{
    public static implicit operator string(ClassifiedAdText value) => value.Value;
}