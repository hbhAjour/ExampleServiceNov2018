namespace ExampleServiceNov2018.Events.TodoList
{
    public class TodoItemAdded : Event
    {
        public readonly int Number;
        public readonly string Text;

        public TodoItemAdded(string aggregateId, string text, int number) : base(aggregateId)
        {
            Text = text;
            Number = number;
        }
    }
}