namespace Marketplace.Framework;

public interface IAggregateStore
{
    Task<bool> Exits<T>(string aggregateId);
    Task Save<T>(T aggregate, string aggregateId) where T : AggregateRoot;
    Task<T> Load<T>(string aggregateId) where T : AggregateRoot;
}