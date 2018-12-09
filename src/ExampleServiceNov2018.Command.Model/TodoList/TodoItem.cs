using ExampleServiceNov2018.Events.TodoList;

namespace ExampleServiceNov2018.Command.Model.TodoList
{
    public class TodoItem : ValueObject<TodoItem>
    {
        public static TodoItem New => new TodoItem(string.Empty, false);
        public readonly bool Done;
        public readonly string Text;

        private TodoItem(string text, bool done)
        {
            Text = text;
            Done = done;
        }

        public override TodoItem Apply(object @event)
        {
            switch (@event)
            {
                case TodoItemAdded added: return new TodoItem(added.Text, Done);
                case TodoItemChecked _: return new TodoItem(Text, true);
                case TodoItemUnchecked _: return new TodoItem(Text, false);
                default: return this;
            }
        }
    }
}