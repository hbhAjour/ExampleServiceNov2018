
namespace ExampleServiceNov2018.Query.Model
{
    public class TodoListCollectionDTO
    {
        public TodoListDTO[] Collection { get; set; }
    }

    public class TodoListDTO
    {
        public string AggregateId { get; set; }
        public string Name { get; set; }
        public TodoItemDTO[] Items { get; set; }
    }

    public class TodoItemDTO
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
    }
}