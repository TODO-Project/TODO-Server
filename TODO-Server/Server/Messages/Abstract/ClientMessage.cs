﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using TODO_Server.Server.Messages.Types;

namespace TODO_Server.Server.Messages.Abstract
{
    /// <summary>
    /// Describes a message sent by the client to the server
    /// </summary>
    public abstract class ClientMessage : IMessage
    {
        /// <summary>
        /// The type of the message
        /// </summary>
        public ClientMessageTypes MessageType { get; set; }

        
        public abstract void DecodeMessage(NetIncomingMessage inc);

        
        public abstract void EncodeMessage(NetOutgoingMessage outmsg);
    }
}
