using System;

namespace ExampleServiceNov2018.Commands
{
    public abstract class CommandResult
    {
        public Guid TransactionId { get; }
        public object Command { get; }

        protected CommandResult(object command)
        {
            TransactionId = Guid.NewGuid();
            Command = command;
        }

        public class Handled : CommandResult
        {
            public DateTime ExecutionTime { get; }
            public Handled(object command): base(command)
            {
                ExecutionTime = DateTime.UtcNow;
            }
        }

        //Rejected, //Failed
    }
}