using System.Windows.Controls;

namespace TODO_Server.Console.Commands
{
    public class EchoCommand : Command
    {

        public string EchoMessage { get; set; }

        public EchoCommand(string message)
        {
            EchoMessage = message;
            Execute();
        }

        public override void Execute()
        {
            ServerConsole.Print(EchoMessage);
        }
    }
}
