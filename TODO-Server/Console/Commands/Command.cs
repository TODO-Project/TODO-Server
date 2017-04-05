using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// Describes a command executed by the ServerConsole
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Executes the command
        /// </summary>
        public abstract void Execute();
    }
}
