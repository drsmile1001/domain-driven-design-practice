using Marketplace.Framework;
using Marketplace.Domain;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api;
public class ClassifiedAdsApplicationService : IApplicationService
{
    private readonly IEntityStore _store;
    private readonly ICurrencyLookup _currencyLookup;


    public ClassifiedAdsApplicationService(IEntityStore store, ICurrencyLookup currencyLookup)
    {
        _store = store;
        _currencyLookup = currencyLookup;
    }

    public Task Handle(object command)
    => command switch
    {
        V1.Create cmd => HandleCreate(cmd),
        V1.SetTitle cmd => HandleUpdate(cmd.Id, c => c.SetTitle(cmd.Title)),
        V1.UpdateText cmd => HandleUpdate(cmd.Id, c => c.UpdateText(new ClassifiedAdText(cmd.Text))),
        V1.UpdatePrice cmd => HandleUpdate(cmd.Id, c =>
                             c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
        V1.RequestToPublish cmd => HandleUpdate(cmd.Id, c => c.RequestToPublish()),
        _ => throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown"),
    };

    private async Task HandleCreate(V1.Create cmd)
    {
        if (await _store.Exists<ClassifiedAd>(cmd.Id.ToString()))
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
        var classifiedAd = new ClassifiedAd(cmd.Id, cmd.OwnerId);
        await _store.Save(classifiedAd);
    }

    private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
    {
        var classifiedAd = await _store.Load<ClassifiedAd>(classifiedAdId.ToString());

        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
        operation(classifiedAd);
        await _store.Save(classifiedAd);
    }
}