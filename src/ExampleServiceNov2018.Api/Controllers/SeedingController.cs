using System;
using System.Threading.Tasks;
using System.Timers;
using ExampleServiceNov2018.Commands.TodoList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleServiceNov2018.Api.Controllers
{
    [Route("test")]
    public class SeedingController : Controller
    {
        private readonly IMediator _mediator;

        public SeedingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("seed/{qty}")]
        public async Task<IActionResult> Seed([FromRoute]int qty)
        {
            //sequential writes
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var ticks = DateTimeOffset.UtcNow.Ticks;
            
            for (int i = 0; i < qty; i++)
            {
                               
                var listId = $"Todo{ticks+i}";
                await _mediator.Send(new NameTodoList{Name = listId, AggragateId = listId});
                await _mediator.Send(new AddTodoItem{ItemText= "a",ItemNumber = 1, AggragateId = listId});
                await _mediator.Send(new AddTodoItem{ItemText= "a2",ItemNumber = 2, AggragateId = listId});
                await _mediator.Send(new CheckTodoItem{ItemNumber = 1, AggragateId = listId});
                await _mediator.Send(new CheckTodoItem{ItemNumber = 2, AggragateId = listId});
                await _mediator.Send(new UncheckTodoItem{ItemNumber= 1, AggragateId = listId});
                await _mediator.Send(new AddTodoItem{ItemText= "a3",ItemNumber = 3, AggragateId = listId});
                await _mediator.Send(new CheckTodoItem {ItemNumber = 3, AggragateId = listId});
                await _mediator.Send(new CheckTodoItem {ItemNumber = 1, AggragateId = listId});
                await _mediator.Send(new NameTodoList(){Name = "Closed", AggragateId = listId});
            }

            watch.Stop();
            return Ok(watch.Elapsed.ToString());

        }
    }
}