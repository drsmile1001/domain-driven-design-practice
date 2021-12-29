using System.Text;
using System.Text.Json;
using EventStore.Client;

namespace Marketplace.Infrastructure;

public static class EventDeserializer
{
    public static object Deserialize(this ResolvedEvent resolvedEvent)
    {
        var meta = JsonSerializer
            .Deserialize<EventMatadata>(Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.Span));
        var dataType = Type.GetType(meta!.ClrType);
        var data = JsonSerializer.Deserialize(resolvedEvent.Event.Data.Span, dataType!);
        return data!;
    }
}