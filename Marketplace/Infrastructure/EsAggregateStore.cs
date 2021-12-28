using System.Text;
using System.Text.Json;
using EventStore.Client;
using Marketplace.Framework;

namespace Marketplace.Infrastructure;

public class EsAggregateStore : IAggregateStore
{
    private readonly EventStoreClient _client;

    public EsAggregateStore(EventStoreClient client)
    {
        _client = client;
    }

    public static string GetStreamName<T>(string aggregateId)
        => $"{typeof(T).Name}-{aggregateId}";

    public async Task Save<T>(T aggregate, string aggregateId)
        where T : AggregateRoot
    {
        var changes = aggregate.GetChanges()
        .Select(@event => new EventData(
            eventId: Uuid.NewUuid(),
            type: @event.GetType().Name,
            data: JsonSerializer.SerializeToUtf8Bytes(@event),
            metadata: JsonSerializer.SerializeToUtf8Bytes(new EventMatadata
            {
                ClrType = @event.GetType().AssemblyQualifiedName!,
            }))).ToArray();
        if (!changes.Any())
        {
            return;
        }

        var streamName = GetStreamName<T>(aggregateId);
        var version = aggregate.Version == ulong.MaxValue ? StreamRevision.None : new StreamRevision(aggregate.Version);
        await _client.AppendToStreamAsync(streamName, version, changes);
        aggregate.ClearChanges();
    }

    public async Task<T> Load<T>(string aggregateId)
        where T : AggregateRoot
    {
        var stream = GetStreamName<T>(aggregateId);
        var aggregate = (T?)Activator.CreateInstance(typeof(T), true);

        var events = await _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start).ToListAsync();

        aggregate!.Load(events.Select(e =>
        {
            var meta = JsonSerializer.Deserialize<EventMatadata>(Encoding.UTF8.GetString(e.Event.Metadata.Span));
            var dataType = Type.GetType(meta!.ClrType);
            var data = JsonSerializer.Deserialize(e.Event.Data.Span, dataType!);
            return data!;
        }).ToArray());

        return aggregate;
    }

    public async Task<bool> Exits<T>(string aggregateId)
    {
        var stream = GetStreamName<T>(aggregateId);
        var state = await _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start, 1).ReadState;
        return state != ReadState.StreamNotFound;
    }

    private class EventMatadata
    {
        public string ClrType { get; init; } = null!;
    }
}