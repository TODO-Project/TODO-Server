using System.Windows.Controls;

namespace TODO_Server.Console.Commands
{
    /// <summary>
    /// A command that echoes a given string
    /// </summary>
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
