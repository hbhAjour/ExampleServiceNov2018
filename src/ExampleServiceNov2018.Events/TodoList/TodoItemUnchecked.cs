namespace ExampleServiceNov2018.Events.TodoList
{

    public class TodoItemUnchecked : Event
    {
        public readonly int Number;

        public TodoItemUnchecked(string aggregateId, int number) : base(aggregateId)
        {
            Number = number;
        }
    }
}