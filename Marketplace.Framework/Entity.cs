namespace Marketplace.Framework;

public abstract class Entity : IInternalEventHandler
{
    private readonly Action<object> _applier;

    protected Entity(Action<object> applier)
    {
        _applier = applier;
    }

    public void Handle(object @event)
        => When(@event);

    protected abstract void When(object @event);

    protected void Apply(object @event)
    {
        When(@event);
        _applier(@event);
    }
}