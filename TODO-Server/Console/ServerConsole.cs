using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TODO_Server.Console.Commands;

namespace TODO_Server.Console
{
    public static class ServerConsole
    {
        public static TextBlock Console;
        public static MainWindow ConsoleWindow;

        public static void Print(string message)
        {  
            ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Console.Text += "\n[" + DateTime.Now.ToLongTimeString() + "] >> " + message;
            }));
        }

        public static void Print(string message, ConsoleFlags flag)

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
                case ConsoleFlags.Debug:
                    res += "[DEBUG] >> ";
                    break;
                default:
                    break;
            }

            res += message;
            Log(res);
            ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Console.Text += "\n" + res;
            }));
        }

        public static bool HandleCommands(string command)
        {
            Print(command);
            string[] args = command.Split(' ');
            switch (args[0].ToLower())
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
                    new EchoCommand(echoContent);
                    return true;
                case "help":
                    new HelpCommand();
                    return true;
                case "exit":
                    new ExitCommand(ConsoleWindow);
                    return true;
                case "seed":
                    new SeedCommand();
                    return true;
                case "clearlog":
                    new ClearLogCommand();
                    return true;
                case "port":
                    new PortCommand();
                    return true;
                default:
                    return false;
            }
        }

        private static void Log(string line)
        {
            System.IO.File.AppendAllText("server.log", "\n" + line);
        }

        public static void ClearLog()
        {
            System.IO.File.WriteAllText("server.log", string.Empty);
            Print("Log was cleared successfully", ConsoleFlags.Info);
        }
    }
}
