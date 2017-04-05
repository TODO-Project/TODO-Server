using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that clears the log (server.log)
    /// </summary>
    public class ClearLogCommand : Command
    {
        public ClearLogCommand()
        {
            Execute();
        }
        public override void Execute()
        {
            ServerConsole.ClearLog();
        }
    }
}
