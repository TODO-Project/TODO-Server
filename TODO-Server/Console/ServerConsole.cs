using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Console
{
    public static class ServerConsole
    {
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

        private static void Log(string line)
        {
            System.IO.File.AppendAllText("server.log", "\n" + line);
        }
    }
}
