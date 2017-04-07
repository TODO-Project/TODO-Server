using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Server;

namespace TODO_Server.Console.Commands
{
    public class KickCommand : Command
    {
        public string Target { get; set; }


        public KickCommand(string target)
        {
            Target = target;
            Execute();
        }

        public override void Execute()
        {
            if (GameServer.DisconnectPlayer(Server.Messages.Types.DisconnectionFlags.Kick, Target))
                ServerConsole.Print("Player " + Target + " has been kicked.");
            else
                ServerConsole.Print("Player " + Target + " could not be kicked : not on the server");
        }
    }
}
