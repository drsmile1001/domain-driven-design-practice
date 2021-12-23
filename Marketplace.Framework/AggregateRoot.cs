namespace Marketplace.Framework;
public abstract class AggregateRoot : IInternalEventHandler
{
    private readonly List<object> _changes;

    protected AggregateRoot() => _changes = new List<object>();

    public void Handle(object @event)
        => When(@event);

    public IEnumerable<object> GetChanges() =>
        _changes.AsEnumerable();

    public void ClearChanges() => _changes.Clear();

    protected void Apply(object @event)
    {
        When(@event);
        EnsureValidState();
        _changes.Add(@event);
    }

    protected void ApplyToEntity(IInternalEventHandler? entity, object @event)
        => entity?.Handle(@event);

    protected abstract void When(object @event);

    protected abstract void EnsureValidState();
}
