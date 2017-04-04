using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Console;
using System.Threading;

namespace TODO_Server.Server
{
    public static class GameServer
    {
        #region Constants

        private const int Tickrate = 66;

        #endregion

        #region Fields 

        private static List<Player> playerList;
        private static NetServer server;
        private static int seed;
        private static Thread serverThread;
        private static string name;
        private static int packetNumber;

        #endregion

        #region Properties

        public static List<Player> PlayerList { get => playerList; set => playerList = value; }
        public static NetServer Server { get => server; private set => server = value; }
        public static int Seed { get => seed; set => seed = value; }
        public static Thread ServerThread { get => serverThread; set => serverThread = value; }
        public static string Name { get => name; set => name = value; }
        public static int PacketNumber { get => ++packetNumber == int.MaxValue ? 0 : packetNumber; }

        #endregion

        #region Methods

        public static void Initialize()
        {
            InitializeSeed();
            InitializeServer();
        }

        private static void InitializeSeed()
        {
            ServerConsole.Print("Initializing game server...", ConsoleFlags.Info);
            Seed = Guid.NewGuid().GetHashCode();
            ServerConsole.Print("Seed generated : " + Seed, ConsoleFlags.Info);
        }

        private static void InitializeServer()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("TODO-Game");
            ServerThread = new Thread(Work);

            config.MaximumConnections = 128;
            config.Port = 12345;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            ServerThread.Name = "Server Thread";
            Name = "TODO-Game server";

            Server = new NetServer(config);

            Server.Start();
            ServerThread.Start();
            ServerConsole.Print("The server has started successfully", ConsoleFlags.Info);
        }

        private static void Work()
        {
            NetIncomingMessage inc;

            while ((inc = Server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        ServerConsole.Print(inc.ReadString(), ConsoleFlags.Alert);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)inc.ReadByte())
                        {
                            case NetConnectionStatus.None:
                                break;
                            case NetConnectionStatus.ReceivedInitiation:
                            case NetConnectionStatus.RespondedAwaitingApproval:
                            case NetConnectionStatus.RespondedConnect:
                            case NetConnectionStatus.Connected:
                            case NetConnectionStatus.Disconnecting:
                            case NetConnectionStatus.Disconnected:
                                ServerConsole.Print(inc.ReadString(), ConsoleFlags.Info);
                                break;
                            default:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        ApproveConnection(inc);
                        break;
                    case NetIncomingMessageType.Data:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        SendDiscoveryResponse(inc);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        ServerConsole.Print(inc.ReadString(), ConsoleFlags.Debug);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        ServerConsole.Print(inc.ReadString(), ConsoleFlags.Alert);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        ServerConsole.Print(inc.ReadString(), ConsoleFlags.Fatal);
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        break;
                }
                Server.Recycle(inc);
            }

            Thread.Sleep(Tickrate);
        }

        private static void SendDiscoveryResponse(NetIncomingMessage inc)
        {
            NetOutgoingMessage msg = Server.CreateMessage();
            msg.Write(Name);

            Server.SendDiscoveryResponse(msg, inc.SenderEndPoint);
        }

        private static void ApproveConnection(NetIncomingMessage inc)
        {
            string secret = inc.ReadString();
            if (secret == "TODO-Game Client")
                inc.SenderConnection.Approve();
            else
                inc.SenderConnection.Deny();
        }

        #endregion

    }
}
