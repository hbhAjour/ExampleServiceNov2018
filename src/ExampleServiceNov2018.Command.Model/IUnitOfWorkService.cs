using System.Threading.Tasks;
using ExampleServiceNov2018.Commands;

namespace ExampleServiceNov2018.Command.Model
{
    public interface IUnitOfWorkService
    {
        Task<TAggregate> Get<TAggregate>(string aggregateId) where TAggregate: AggregateState, new();
        Task<TAggregate> View<TAggregate>(string aggregateId) where TAggregate : AggregateState, new();
        Task<CommandResult> EmitEventsFor(CommandBase command);
    }
}