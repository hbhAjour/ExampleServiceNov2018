namespace ExampleServiceNov2018.Events.TodoList
{
    public class TodoItemChecked : Event
    {
        public readonly int Number;

        public TodoItemChecked(string aggregateId, int number) : base(aggregateId)
        {
            Number = number;
        }
    }
}