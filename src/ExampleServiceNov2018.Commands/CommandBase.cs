using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleServiceNov2018.Commands
{
    public abstract class CommandBase: IRequest<CommandResult>
    {
    }
}
