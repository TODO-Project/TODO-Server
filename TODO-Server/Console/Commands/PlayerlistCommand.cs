using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Server;

namespace TODO_Server.Console.Commands
{
    public class PlayerlistCommand : Command
    {
        public PlayerlistCommand()
        {
            Execute();
        }

        public override void Execute()
        {
            ServerConsole.Print("Total : " + GameServer.PlayerList.Count + " player(s)");
            foreach (var p in GameServer.PlayerList)
            {   
                ServerConsole.Print(" - " + p.ToString());
            }
        }
    }
}
