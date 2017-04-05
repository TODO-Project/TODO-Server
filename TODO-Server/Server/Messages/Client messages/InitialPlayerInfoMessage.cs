using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using TODO_Server.Server.Messages.Abstract;

namespace TODO_Server.Server.Messages.ClientMessages
{
    public class InitialPlayerInfoMessage : ClientMessage
    {
        public string Name { get; set; }
        public int TeamNumber { get; set; }
        public string Weapon { get; set; }
        public string Class { get; set; }

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
