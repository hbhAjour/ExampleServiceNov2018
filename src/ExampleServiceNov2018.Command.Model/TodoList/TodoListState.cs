using System.Collections.Generic;
using ExampleServiceNov2018.Events.TodoList;

namespace ExampleServiceNov2018.Command.Model.TodoList
{
    public class TodoListState : AggregateState
    {
        private readonly Dictionary<int, TodoItem> _items = new Dictionary<int, TodoItem>();
        public string Name { get; private set; } = string.Empty;
        public IReadOnlyDictionary<int, TodoItem> Items => _items;

        protected override void When(object @event)
        {
            switch (@event)
            {
                case TodoListNamed e:
                    Name = e.Name;
                    break;
                case TodoItemAdded e:
                    UpdateItem(e.Number, e);
                    break;
                case TodoItemChecked e:
                    UpdateItem(e.Number, e);
                    break;
                case TodoItemUnchecked e:
                    UpdateItem(e.Number, e);
                    break;
            }
        }

        private void UpdateItem(int number, object @event)
        {
            var item = Items.TryGetValue(number, out var existing)
                ? existing.Apply(@event)
                : TodoItem.New.Apply(@event);

            _items[number] = item;
        }
    }
}