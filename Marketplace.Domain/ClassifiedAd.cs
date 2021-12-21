namespace Marketplace.Domain;
public class ClassifiedAd
{
    public Guid Id { get; }

    public ClassifiedAd(Guid id)
    {
        if (id == default)
            throw new ArgumentException("id must be set!", nameof(id));
        Id = id;
    }

    private Guid _ownerId;
    private string _title;
    private string _text;
    private decimal _price;
}
