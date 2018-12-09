namespace ExampleServiceNov2018.Commands.TodoList
{
    public class AddTodoItem : SingleAggregateCommand
    {
        public int ItemNumber { get; set; }
        public string ItemText { get; set; }
    }
}