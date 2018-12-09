namespace ExampleServiceNov2018.Commands.TodoList
{

    public class UncheckTodoItem : SingleAggregateCommand
    {
        public int ItemNumber { get; set; }
    }
}