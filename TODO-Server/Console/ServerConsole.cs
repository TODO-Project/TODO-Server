using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TODO_Server.Console.Commands;
using TODO_Server.Server;

namespace TODO_Server.Console
{
    /// <summary>
    /// Describes the server console. Displays numerous infos about the current
    /// status of the server, players, and is capable to execute some commands.
    /// </summary>
    public static class ServerConsole
    {
        /// <summary>
        /// The console on which it writes
        /// </summary>
        public static TextBlock Console;

        /// <summary>
        /// The main window
        /// </summary>
        public static MainWindow ConsoleWindow;

        /// <summary>
        /// Prints a single message on screen that will not be logged
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void Print(string message)
        {  
            ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Console.Text += "\n[" + DateTime.Now.ToLongTimeString() + "] >> " + message;
            }));
        }


        /// <summary>
        /// Prints a message with a flag tag that will be logged
        /// </summary>
        /// <param name="message">The message to be printed</param>
        /// <param name="flag">The flag describing the message</param>
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


        /// <summary>
        /// Handles the commands given by the server admin and
        /// executes them if any
        /// </summary>
        /// <param name="command">The command prompted</param>
        /// <returns>A boolean depending on wether a valid command was given or not</returns>
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
                    new EchoCommand(AggregateArgs(args, 1));
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
                case "kick":
                    new KickCommand(AggregateArgs(args, 1));
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Aggregates command arguments to return them as a whole, starting from an offset
        /// </summary>
        /// <param name="args">The argument array to aggregate</param>
        /// <param name="offset">The index where aggregation should begin</param>
        /// <returns></returns>
        private static string AggregateArgs(string[] args, int offset)
        {
            string res = "";
            for (int i = offset; i < args.Length; i++)
            {
                res += args[i];
                if (i != args.Length - 1)
                    res += " ";
            }
            return res;
        }

        /// <summary>
        /// Logs the last message printed on screen in the server.log file
        /// </summary>
        /// <param name="line">The message to be logged</param>
        private static void Log(string line)
        {
            System.IO.File.AppendAllText("server.log", "\n" + line);
        }

        /// <summary>
        /// Clears the log
        /// </summary>
        public static void ClearLog()
        {
            System.IO.File.WriteAllText("server.log", string.Empty);
            Print("Log was cleared successfully", ConsoleFlags.Info);
        }

        /// <summary>
        /// Adds a new player to the player list
        /// </summary>
        /// <param name="p">The player to add</param>
        public static void AddNewPlayerToList(Player p)
        {
            ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ConsoleWindow.ListViewPlayerList.Items.Add(p);
            }));
            ServerConsole.Print("Added player " + p.Name + "(" + p.ID + ") to server list", ConsoleFlags.Debug);
        }
    }
}
