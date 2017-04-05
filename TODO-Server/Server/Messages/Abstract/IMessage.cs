using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Server.Messages
{
    public interface IMessage
    {
        void DecodeMessage(NetIncomingMessage inc);

        void EncodeMessage(NetOutgoingMessage outmsg);

    }
}
