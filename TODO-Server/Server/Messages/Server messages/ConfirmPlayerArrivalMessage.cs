using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using TODO_Server.Server.Messages.Abstract;

namespace TODO_Server.Server.Messages.Server_messages
{
    /// <summary>
    /// A message sent by the server to the client who sent the previous
    /// InitialPlayerInfoMessage, confirming the arrival to the server
    /// and used by the client to determine wether or not it has been
    /// accepted onto the server
    /// </summary>
    public class ConfirmPlayerArrivalMessage : ServerMessage
    {
        public ConfirmPlayerArrivalMessage()
        {
            MessageType = Types.ServerMessageTypes.ConfirmArrivalOnServer;
        }
        public override void DecodeMessage(NetIncomingMessage inc)
        {
            return;
        }

        public override void EncodeMessage(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)MessageType);
        }
    }
}
