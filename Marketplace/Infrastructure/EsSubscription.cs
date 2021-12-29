using EventStore.Client;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;

namespace Marketplace.Infrastructure;

public class EsSubscription : BackgroundService
{
    private readonly EventStoreClient _client;
    private readonly IList<ReadModels.ClassifiedAdDetails> _items;
    private StreamSubscription? _subscription;

    public EsSubscription(EventStoreClient client, IList<ReadModels.ClassifiedAdDetails> items)
    {
        _client = client;
        _items = items;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _subscription?.Dispose();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscription = await _client.SubscribeToAllAsync(EventAppeared, cancellationToken: stoppingToken);
    }

    private Task EventAppeared(StreamSubscription subscription, ResolvedEvent resolvedEvent, CancellationToken cancellationToken)
    {
        if (!resolvedEvent.Event.EventType.StartsWith("ClassifiedAd"))
        {
            return Task.CompletedTask;
        }

        var @event = resolvedEvent.Deserialize();
        Console.WriteLine($"Projecting event {@event.GetType().Name}");
        switch (@event)
        {
            case Events.ClassifiedAdCreated e:
                _items.Add(new ReadModels.ClassifiedAdDetails
                {
                    ClassifiedAdId = e.Id,
                });
                break;
            case Events.ClassifiedAdTitleChanged e:
                UpdateItem(e.Id, ad => ad.Title = e.Title);
                break;
            case Events.ClassifiedAdTextChanged e:
                UpdateItem(e.Id, ad => ad.Description = e.AdText);
                break;
            case Events.ClassifiedAdPriceUpdated e:
                UpdateItem(e.Id, ad =>
                {
                    ad.Price = e.Price;
                    ad.CurrencyCode = e.CurrencyCode;
                });
                break;
            default:
                break;
        }

        return Task.CompletedTask;
    }

    private void UpdateItem(Guid id, Action<ReadModels.ClassifiedAdDetails> update)
    {
        var item = _items.FirstOrDefault(x => x.ClassifiedAdId == id);
        if (item == null)
        {
            return;
        }

        update(item);
    }
}