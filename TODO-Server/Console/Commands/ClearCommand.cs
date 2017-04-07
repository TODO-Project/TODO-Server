using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that clears the console
    /// </summary>
    public class ClearCommand : Command
    {
        public TextBlock Target
        {
            get;
            set;
        }

        public ClearCommand(TextBlock target)
        {
            Target = target;
            Execute();
        }

        public override void Execute()
        {
            Target.Text = "";
        }
    }
}
