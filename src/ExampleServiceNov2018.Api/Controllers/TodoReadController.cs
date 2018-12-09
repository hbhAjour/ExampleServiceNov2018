using System.Threading.Tasks;
using ExampleServiceNov2018.Query.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleServiceNov2018.Api.Controllers
{
    [Route("todo/read")]
    public class TodoReadController : Controller
    {
        protected ITodoReadService ToDoReader { get; }

        public TodoReadController(ITodoReadService toDoReader)
        {
            ToDoReader = toDoReader;
        }

        [HttpGet("All")]
        public async Task<TodoListCollectionDTO> GetAll()
        {
            return await ToDoReader.ListAll();
        }

        [HttpGet("ById/{aggregateId}")]
        public async Task<TodoListDTO> GetByIId([FromRoute] string aggregateId)
        {
            return await ToDoReader.GetByAggregateId(aggregateId);
        }
    }
}