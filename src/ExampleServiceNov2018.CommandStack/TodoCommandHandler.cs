using System.Threading;
using System.Threading.Tasks;
using ExampleServiceNov2018.Command.Model;
using ExampleServiceNov2018.Command.Model.TodoList;
using ExampleServiceNov2018.Commands.TodoList;
using ExampleServiceNov2018.Commands;
using MediatR;

namespace ExampleServiceNov2018.CommandStack
{
    /// <summary>
    ///     Unwraps routing-commandse
    /// </summary>
    public class TodoCommandHandler : CommandHandler<TodoListState>,
        IRequestHandler<NameTodoList, CommandResult>,
        IRequestHandler<AddTodoItem, CommandResult>,
        IRequestHandler<CheckTodoItem, CommandResult>,
        IRequestHandler<UncheckTodoItem, CommandResult>
    {
        public TodoCommandHandler(IUnitOfWorkService state) : base(state)
        {

        }

        public async Task<CommandResult> Handle(AddTodoItem c, CancellationToken t)
        {
            return await Execute(c, todoList => todoList.AddTodoItem(c.ItemNumber, c.ItemText));
        }

        public async Task<CommandResult> Handle(CheckTodoItem c, CancellationToken t)
        {
            return await Execute(c, todoList => todoList.CheckTodoItem(c.ItemNumber));
        }

        public async Task<CommandResult> Handle(NameTodoList c, CancellationToken t)
        {
            return await Execute(c, todoList => todoList.NameTodoList(c.Name));
        }

        public async Task<CommandResult> Handle(UncheckTodoItem c, CancellationToken t)
        {
            return await Execute(c, todoList => todoList.UncheckTodoItem(c.ItemNumber));
        }

        public async Task<CommandResult> Handle(CommandBase c, CancellationToken t)
        {
            // Multi aggregate command
            //var todoList = await _service.Get<TodoListState>("id");
            //var readAggregate = await _service.View<AggregateState>("id1");
            //var writeAggregate = await _service.Get<AggregateState>("id2");
            // perform command
            return await _service.EmitEventsFor(c);
        }
    }
}