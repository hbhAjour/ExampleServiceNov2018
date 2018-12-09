using System;
using System.Threading.Tasks;
using ExampleServiceNov2018.Command.Model;
using ExampleServiceNov2018.Commands;

namespace ExampleServiceNov2018.CommandStack
{
    public abstract class CommandHandler<TAggregate> where TAggregate: AggregateState, new()
    {
        protected readonly IUnitOfWorkService _service;

        protected CommandHandler(IUnitOfWorkService service)
        {
            _service = service;
        }
        protected async Task<CommandResult> Execute(SingleAggregateCommand command, Action<TAggregate> action)
        {
            action(await _service.Get<TAggregate>(command.AggragateId));
            return await _service.EmitEventsFor(command);
        }
    }
}