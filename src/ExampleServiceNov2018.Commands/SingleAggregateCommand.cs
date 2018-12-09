using ExampleServiceNov2018.Values;

namespace ExampleServiceNov2018.Commands
{
    public abstract class SingleAggregateCommand: CommandBase
    {
        public AggragateId AggragateId { get; set; }
    }
}