using EventStore.Client;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Framework;

namespace Marketplace.Infrastructure;

public class ProjectionManager : BackgroundService
{
    private readonly EventStoreClient _client;
    private readonly IProjection[] _projections;
    private StreamSubscription? _subscription;

    public ProjectionManager(EventStoreClient client, params IProjection[] projections)
    {
        _client = client;
        _projections = projections;
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
        if (resolvedEvent.Event.EventType.StartsWith("$"))
        {
            return Task.CompletedTask;
        }

        object? @event;

        try
        {
            @event = resolvedEvent.Deserialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"can not deserialize ${resolvedEvent.Event.EventType}, {ex.Message}");
            return Task.CompletedTask;
        }

        Console.WriteLine($"Projecting event {@event.GetType().Name}");

        return Task.WhenAll(_projections.Select(x => x.Project(@event)));
    }
}