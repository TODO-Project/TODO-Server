using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Server.Messages
{
    /// <summary>
    /// Describes a network message
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Decodes an incoming message into the object
        /// </summary>
        /// <param name="inc">The incoming message</param>
        void DecodeMessage(NetIncomingMessage inc);

        /// <summary>
        /// Encodes an outgoing message with the object's infos
        /// </summary>
        /// <param name="outmsg">The outgoing message</param>
        void EncodeMessage(NetOutgoingMessage outmsg);

    }
}
