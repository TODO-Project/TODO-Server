using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Server;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that displays the randomly generated seed
    /// </summary>
    public class SeedCommand : Command
    {
        public SeedCommand()
        {
            Execute();
        }

        public override void Execute()
        {
            ServerConsole.Print("Seed : " + GameServer.Seed);
        }
    }
}
