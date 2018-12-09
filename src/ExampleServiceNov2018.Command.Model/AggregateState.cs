using ExampleServiceNov2018.Events;
using ExampleServiceNov2018.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleServiceNov2018.Command.Model
{
    /// <summary>
    ///     Consider making an immutable version?
    /// </summary>
    public abstract class AggregateState
    {
        private List<Event> _uncomittedEvents;
        public AggragateId Id { get; private set; }
        public int LoadedRevision { get; private set; }
        public bool IsReadOnly { get; private set; }

        protected AggregateState()
        {
            IsReadOnly = true;
        }

        public static TAggregate Build<TAggregate>(AggragateId id, int loadedRevision, bool isReadOnly, IEnumerable<object> events) where TAggregate: AggregateState, new()
        {
            var state = new TAggregate
            {
                Id = id,
                LoadedRevision = loadedRevision,
                IsReadOnly = isReadOnly
            };
            state._uncomittedEvents = state.IsReadOnly ? null : new List<Event>();
            foreach (var @event in events)
            {
                state.When(@event);
            }
            return state;
        }

        public IEnumerable<Event> EmitUncommittedEvents(Guid transactionId)
        {
            var events = _uncomittedEvents.Select(x => x.SetTransactionContext(transactionId)).ToArray();
            _uncomittedEvents.Clear();
            return events;
        }        

        internal void Apply(Event @event)
        {
            When(@event);
            _uncomittedEvents.Add(@event);
        }
        
        protected abstract void When(object @event); // Should be private prodected with C# 7.2
    }
}