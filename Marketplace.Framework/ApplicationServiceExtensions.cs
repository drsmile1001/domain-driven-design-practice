namespace Marketplace.Framework;

public static class ApplicationServiceExtensions
{
    public static async Task HandleUpdate<T>(
        this IApplicationService service,
        IAggregateStore store,
        string aggregateId,
        Action<T> operation)
        where T : AggregateRoot
    {
        var aggregate = await store.Load<T>(aggregateId);
        if (aggregate == null)
        {
            throw new InvalidOperationException($"Entity with id {aggregateId} cannot be found");
        }

        operation(aggregate);
        await store.Save(aggregate, aggregateId);
    }
}