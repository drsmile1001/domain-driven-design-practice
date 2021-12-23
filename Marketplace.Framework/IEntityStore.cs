namespace Marketplace.Framework;

public interface IEntityStore
{
    Task<T?> Load<T>(string entityId);

    Task Save<T>(T entity);

    Task<bool> Exists<T>(string entityId);
}