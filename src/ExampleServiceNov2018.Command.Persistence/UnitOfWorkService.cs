using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleServiceNov2018.Command.Model;
using ExampleServiceNov2018.Commands;
using ExampleServiceNov2018.Events;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;
using StreamStoreStore.Json;

namespace ExampleServiceNov2018.Command.Persistense
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();
        private readonly IStreamStore _store;
        private readonly List<AggregateState> _trackedAggregates;

        public UnitOfWorkService(IStreamStore store)
        {
            _store = store;
            _trackedAggregates = new List<AggregateState>();
        }

        protected async Task<TAggregate> Load<TAggregate>(string aggregateId, bool isReadOnly = false) where TAggregate : AggregateState, new()
        {
            var stream = await _store.ReadStreamForwards(new StreamId(aggregateId), 0, int.MaxValue, true);
            var events = new List<object>();
            foreach (var message in stream.Messages)
            {
                var @event = await Deserialize(message);
                events.Add(@event);
            }
            //var events = await Task.WhenAll(stream.Messages.Select(Deserialize));
            var aggregate = AggregateState.Build<TAggregate>(aggregateId, stream.LastStreamVersion, isReadOnly, events); 
            if (!isReadOnly)
            {
                _trackedAggregates.Add(aggregate);
            }
            return aggregate;
        }

        public async Task<TAggregate> Get<TAggregate>(string aggregateId) where TAggregate : AggregateState, new()
        {
            return await Load<TAggregate>(aggregateId, false);
        }
        public async Task<TAggregate> View<TAggregate>(string aggregateId) where TAggregate : AggregateState, new()
        {
            return await Load<TAggregate>(aggregateId, true);
        }
        public async Task<CommandResult> EmitEventsFor(CommandBase command)
        {
            try
            {
                var commandResult = new CommandResult.Handled(command);
                if (!_trackedAggregates.Any())
                {
                    return commandResult;
                }
                else if (_trackedAggregates.Count == 1)
                {
                    var appendResults = await Save(_trackedAggregates.First(), commandResult.TransactionId);
                }
                else
                {
                    var appendResults = await Task.WhenAll(
                        _trackedAggregates.Select(aggregate => Save(aggregate, commandResult.TransactionId)));
                }
                // commandResult should also be persisted to a commandstore/stream
                _trackedAggregates.Clear();
                return commandResult;
            }
            catch (Exception e)
            {
                // Ensure (eventual) consistency, if data access lib cant handle multiple aggregate commits (SQL should be though)
                // return failed commandResult
                throw;
            }
        }

        private async Task<AppendResult> Save(AggregateState aggregate, Guid transactionId)
        {
            var toAppend = aggregate.EmitUncommittedEvents(transactionId).Select(Serialize).ToArray();
            return await _store.AppendToStream(aggregate.Id.ToString(), aggregate.LoadedRevision, toAppend);
        }

        private NewStreamMessage Serialize(Event @event)
        {
            return new NewStreamMessage(
                Guid.NewGuid(),
                @event.GetType().Name,
                SimpleJson.SerializeObject(@event));
        }


        private async Task<object> Deserialize(StreamMessage message)
        {
            if (!_typeCache.TryGetValue(message.Type, out var eventType))
            {
                //Guess type by convention:
                var typename = $"ExampleServiceNov2018.Domain.Events.{message.Type}, ExampleServiceNov2018.Domain";

                eventType = Type.GetType(typename);
                _typeCache[message.Type] = eventType;
            }

            var json = await message.GetJsonData();

            return JsonConvert.DeserializeObject(json, eventType);

            //SimpleJson throws NRE on deserialization!
            //NullReferenceException: Object reference not set to an instance of an object.
            //  StreamStoreStore.Json.PocoJsonSerializerStrategy.DeserializeObject(object value, Type type)
            //  StreamStoreStore.Json.SimpleJson.DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
            //  StreamStoreStore.Json.SimpleJson.DeserializeObject(string json, Type type)
        }
    }
}