using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using TODO_Server.Server.Messages.Abstract;

namespace TODO_Server.Server.Messages.ClientMessages
{
    /// <summary>
    /// A message containing player informations, to be sent once and
    /// is used by the server to add this player to the list
    /// </summary>
    public class InitialPlayerInfoMessage : ClientMessage
    {
        /// <summary>
        /// The player's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The player's team number
        /// </summary>
        public int TeamNumber { get; set; }
        
        /// <summary>
        /// The player's weapon
        /// </summary>
        public string Weapon { get; set; }

        /// <summary>
        /// The player's class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// The player's unique ID
        /// </summary>
        public long ID { get; set; }


        public InitialPlayerInfoMessage(string name, int teamNumber, string weapon, string playerClass, long Id)
        {
            MessageType = Types.ClientMessageTypes.SendInitialPlayerInfo;
            Name = name;
            TeamNumber = teamNumber;
            Weapon = weapon;
            Class = playerClass;
            ID = Id;
        }

        public InitialPlayerInfoMessage()
            : this ("", 0, "", "", 0)
        { }

        public override void DecodeMessage(NetIncomingMessage inc)
        {
            Name = inc.ReadString();
            TeamNumber = inc.ReadInt32();
            Weapon = inc.ReadString();
            Class = inc.ReadString();
            ID = inc.ReadInt64();
        }

        public override void EncodeMessage(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)MessageType);
            outmsg.Write(Name);
            outmsg.Write(TeamNumber);
            outmsg.Write(Weapon);
            outmsg.Write(Class);
            outmsg.Write(ID);
        }
    }
}
