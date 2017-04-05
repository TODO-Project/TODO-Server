using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Server;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that displays the port the server is listening to
    /// </summary>
    public class PortCommand : Command
    {
        public PortCommand()
        {
            Execute();
        }

        public override void Execute()
        {
            ServerConsole.Print(GameServer.Server.Port.ToString());
        }
    }
}
