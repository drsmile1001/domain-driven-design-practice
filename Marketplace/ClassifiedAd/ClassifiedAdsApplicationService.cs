using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using static Marketplace.ClassifiedAd.Commands;

namespace Marketplace.ClassifiedAd;
public class ClassifiedAdsApplicationService : IApplicationService
{
    private readonly IAggregateStore _store;
    private readonly ICurrencyLookup _currencyLookup;

    public ClassifiedAdsApplicationService(
        ICurrencyLookup currencyLookup,
        IAggregateStore store)
    {
        _currencyLookup = currencyLookup;
        _store = store;
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
        V1.Publish cmd => HandleUpdate(cmd.Id, c => c.Publish(cmd.Id)),
        _ => throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown"),
    };

    private async Task HandleCreate(V1.Create cmd)
    {
        if (await _store.Exits<Domain.ClassifiedAd.ClassifiedAd>(cmd.Id.ToString()))
        {
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
        }

        var classifiedAd = new Domain.ClassifiedAd.ClassifiedAd(cmd.Id, cmd.OwnerId);
        await _store.Save(classifiedAd, classifiedAd.Id.Value.ToString());
    }

    private Task HandleUpdate(Guid classifiedAdId, Action<Domain.ClassifiedAd.ClassifiedAd> operation)
        => this.HandleUpdate(_store, classifiedAdId.ToString(), operation);
}