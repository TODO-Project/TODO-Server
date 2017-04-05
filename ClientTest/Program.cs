using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NetPeerConfiguration conf = new NetPeerConfiguration("TODO-Game");    
            NetClient client = new NetClient(conf);

            client.Start();

            NetOutgoingMessage outmsg = client.CreateMessage();
            outmsg.Write("TODO-Game Client");

            client.Connect(new System.Net.IPEndPoint(new System.Net.IPAddress(new byte[] { 192, 168, 56, 1 }), 12345), outmsg);
            
            while(true)
            {
                NetIncomingMessage inc;
                while ((inc = client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Error:
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            break;
                        case NetIncomingMessageType.Data:
                            break;
                        case NetIncomingMessageType.Receipt:
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            break;
                        case NetIncomingMessageType.DiscoveryResponse:
                            Console.WriteLine("Discovery response !");
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            break;
                        case NetIncomingMessageType.NatIntroductionSuccess:
                            break;
                        case NetIncomingMessageType.ConnectionLatencyUpdated:
                            break;
                        default:
                            Console.WriteLine("Unhandled message type : " + inc.MessageType);
                            break;
                    }

                    client.Recycle(inc);
                }
            }
        }
    }
}
