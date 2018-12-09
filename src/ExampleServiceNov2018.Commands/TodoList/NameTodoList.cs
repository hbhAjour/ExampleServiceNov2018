namespace ExampleServiceNov2018.Commands.TodoList
{
    public class NameTodoList : SingleAggregateCommand
    {
        public string Name { get; set; }
    }
}