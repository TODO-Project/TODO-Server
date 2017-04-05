using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Console
{
    /// <summary>
    /// Describes numerous flags that will be displayed before a message
    /// on the server console
    /// </summary>
    public enum ConsoleFlags
    {
        /// <summary>
        /// An information about something that happened
        /// </summary>
        Info,

        /// <summary>
        /// An alert that could be potentially important
        /// </summary>
        Alert,

        /// <summary>
        /// An error that often leads to an exception
        /// </summary>
        Fatal,

        /// <summary>
        /// A debug message (used to debug)
        /// </summary>
        Debug
    }
}
