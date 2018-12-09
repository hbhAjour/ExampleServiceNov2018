using ExampleServiceNov2018.Events.TodoList;
using ExampleServiceNov2018.Events;
using System;

namespace ExampleServiceNov2018.Command.Model.TodoList
{
    /// <summary>
    ///     This is invariant-checking, notice state is method-injected (as can dependencies)
    ///     We expect command to have been structurally validated at this point, and state's id to match the commands
    /// </summary>
    public static class TodoList
    {
        public static void NameTodoList(this TodoListState todoList, string name)
        {
            todoList.Apply(new TodoListNamed(todoList.Id, name));
        }

        public static void AddTodoItem(this TodoListState todoList, int itemNumber, string itemText)
        {
            if (todoList.Items.ContainsKey(itemNumber))
                throw new ArgumentException("This list already has an item with that number");
            todoList.Apply(new TodoItemAdded(todoList.Id, itemText, itemNumber));
        }

        public static void CheckTodoItem(this TodoListState todoList, int itemNumber)
        {
            todoList.Apply(new TodoItemChecked(todoList.Id, itemNumber));
        }

        public static void UncheckTodoItem(this TodoListState todoList, int itemNumber)
        {
            todoList.Apply(new TodoItemUnchecked(todoList.Id, itemNumber));
        }        
    }
}