using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Console.Commands
{
    public abstract class Command
    {
        public abstract bool Execute();
    }
}
