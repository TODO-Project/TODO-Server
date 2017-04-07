using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that exists the server
    /// </summary>
    public class ExitCommand : Command
    {
        public Window Target { get; set; }
        public ExitCommand(Window target)
        {
            Target = target;
            Execute();
        }

        public override void Execute()
        {
            Target.Close();
        }
    }
}
