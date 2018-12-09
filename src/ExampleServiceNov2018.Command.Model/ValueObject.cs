namespace ExampleServiceNov2018.Command.Model
{
    public abstract class ValueObject<T>
    {
        public abstract T Apply(object @event);
    }
}