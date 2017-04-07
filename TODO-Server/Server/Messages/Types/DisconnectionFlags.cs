using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Server.Messages.Types
{
    /// <summary>
    /// Describes the disconnection context of a player.
    /// </summary>
    public enum DisconnectionFlags
    {
        /// <summary>
        /// No context : is default, should not be used.
        /// </summary>
        None,
        
        /// <summary>
        /// The player has been kicked by the server and has been
        /// removed from the match.
        /// </summary>
        Kick
    }
}
