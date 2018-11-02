﻿using System.Threading.Tasks;
using ExampleServiceNov2018.Application;
using ExampleServiceNov2018.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleServiceNov2018.Api.Controllers
{
    [Route("todo/write")]
    public class TodoController : Controller
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NameTodoList")]
        public async Task<CommandResult> NameTodoItem([FromBody]Cmd<NameTodoList> cmd) => await _mediator.Send(cmd);
        
        [HttpPost("AddTodoItem")]
        public async Task<CommandResult> AddTodoItem(Cmd<AddTodoItem> cmd) => await _mediator.Send(cmd);
        
        [HttpPost("CheckTodoItem")]
        public async Task<CommandResult> NewTodoList(Cmd<CheckTodoItem> cmd) => await _mediator.Send(cmd);
        
        [HttpPost("UncheckTodoItem")]
        public async Task<CommandResult> NewTodoList(Cmd<UncheckTodoItem> cmd) => await _mediator.Send(cmd);
    }
}