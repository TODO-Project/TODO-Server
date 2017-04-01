using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TODO_Server.Console.Commands;

namespace TODO_Server.Console
{
    public static class ServerConsole
    {
        public static TextBlock Console;

        public static string Print(string message)
        {
            return "[" + DateTime.Now.ToLongTimeString() + "] >> " + message;
        }

        public static string Print(string message, ConsoleFlags flag)
        {
            string res = "[" + DateTime.Now.ToLongTimeString() + "] ";
            switch (flag)
            {
                case ConsoleFlags.Info:
                    res += "[INFO] >> ";
                    break;
                case ConsoleFlags.Alert:
                    res += "[ALERT] >> ";
                    break;
                case ConsoleFlags.Fatal:
                    res += "[FATAL] >> ";
                    break;
                default:
                    break;
            }

            res += message;
            Log(res);
            return res;
        }

        public static bool HandleCommands(string command)
        {
            string[] args = command.Split(' ');
            switch (args[0])
            {
                case "cls":
                case "clear":
                    new ClearCommand(Console);
                    return true;
                case "echo":
                    string echoContent = "";
                    for(int i = 1; i < args.Length; i++)
                    {
                        echoContent += args[i] + " ";
                    }
                    new EchoCommand(Console, echoContent);
                    return true;
                case "help":
                    new HelpCommand(Console);
                    return true;
                default:
                    return false;
            }
        }

        private static void Log(string line)
        {
            System.IO.File.AppendAllText("server.log", "\n" + line);
        }
    }
}
