using System.Threading;
using System.Threading.Tasks;

namespace ExampleServiceNov2018.Query.Model
{
    public interface ITodoReadService
    {
        Task<TodoListDTO> GetByAggregateId(string aggregateId);
        Task<TodoListCollectionDTO> ListAll();
    }
}