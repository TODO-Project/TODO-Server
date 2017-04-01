using System.Windows.Controls;

namespace TODO_Server.Console.Commands
{
    public class EchoCommand : Command
    {
        public TextBlock Target { get; set; }

        public string EchoMessage { get; set; }

        public EchoCommand(TextBlock target, string message)
        {
            Target = target;
            EchoMessage = message;
            Execute();
        }

        public override bool Execute()
        {
            Target.Text += "\n" + ServerConsole.Print(EchoMessage);
            return false;
        }
    }
}
