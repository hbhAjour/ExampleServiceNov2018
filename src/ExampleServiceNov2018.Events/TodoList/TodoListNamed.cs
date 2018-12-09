namespace ExampleServiceNov2018.Events.TodoList
{
    public class TodoListNamed : Event
    {
        public readonly string Name;

        public TodoListNamed(string aggregateId, string name) : base(aggregateId)
        {
            Name = name;
        }
    }
}