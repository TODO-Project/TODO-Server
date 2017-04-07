using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using TODO_Server.Server.Messages.Abstract;
using TODO_Server.Server.Messages.Types;

namespace TODO_Server.Server.Messages.Server_messages
{
    /// <summary>
    /// A message sent by the server to confirm the disconnection of a player.
    /// The disconnection context is stored in the DisconnectionFlags enum.
    /// </summary>
    class DisconnectedFromServerMessage : ServerMessage
    {
        /// <summary>
        /// The flag describing the disconnection context
        /// </summary>
        public DisconnectionFlags Flag { get; set; }

        /// <summary>
        /// The ID of the player being disconnected
        /// </summary>
        public long ID { get; set; }

        public DisconnectedFromServerMessage(DisconnectionFlags flag, long id)
        {
            MessageType = ServerMessageTypes.DisconnectedFromServer;
            Flag = flag;
            ID = id;
        }

        public DisconnectedFromServerMessage()
            : this(DisconnectionFlags.None, 0)
        { }

        public override void DecodeMessage(NetIncomingMessage inc)
        {
            Flag = (DisconnectionFlags)inc.ReadByte();
            ID = inc.ReadInt64();
        }

        public override void EncodeMessage(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)MessageType);
            outmsg.Write((byte)Flag);
            outmsg.Write(ID);
        }
    }
}
