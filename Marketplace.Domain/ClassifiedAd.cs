namespace Marketplace.Domain;
public class ClassifiedAd
{
    public ClassifiedAdId Id { get; }
    private UserId _ownerId;

    public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
    {
        Id = id;
        _ownerId = ownerId;
    }

    public void SetTitle(string title) => _title = title;
    public void UpdateText(string text) => _text = text;
    public void UpdatePrice(Price price) => _price = price;


    private string _title;
    private string _text;
    private Price _price;
}
