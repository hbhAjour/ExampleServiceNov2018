using ExampleServiceNov2018.Values;
using System;

namespace ExampleServiceNov2018.Events
{
    public abstract class Event
    {
        public string AggregateId{ get; }
        public Guid TransactionId { get; private set; }
        protected Event(AggragateId aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Event SetTransactionContext(Guid transactionId)
        {
            if (TransactionId != Guid.Empty)
            {
                throw new InvalidOperationException();
            }
            TransactionId = transactionId;
            return this;
        }
    }
}