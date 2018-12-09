namespace ExampleServiceNov2018.Commands.TodoList
{
    public class CheckTodoItem : SingleAggregateCommand
    {
        public int ItemNumber { get; set; }
    }
}