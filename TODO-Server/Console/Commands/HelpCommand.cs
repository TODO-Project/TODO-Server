using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TODO_Server.Console.Commands
{
    public class HelpCommand : Command
    {
        public TextBlock Target { get; set; }

        public HelpCommand(TextBlock target)
        {
            Target = target;
            Execute();
        }

        public override bool Execute()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Help menu");
            sb.AppendLine("\techo <message> : Print the message");
            sb.AppendLine("\tclear/cls : Clear the console");
            sb.AppendLine("\thelp : Display this message");
            Target.Text += ServerConsole.Print(sb.ToString());
            return false;
        }
    }
}
