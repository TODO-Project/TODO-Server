using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Console;
using System.Threading;
using System.Windows;
using TODO_Server.Server.Messages.Types;
using TODO_Server.Server.Messages.ClientMessages;
using TODO_Server.Server.Messages.Server_messages;

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
        private static volatile bool shouldRun;

        #endregion

        #region Properties

        public static List<Player> PlayerList { get => playerList; set => playerList = value; }
        public static NetServer Server { get => server; private set => server = value; }
        public static int Seed { get => seed; set => seed = value; }
        public static Thread ServerThread { get => serverThread; set => serverThread = value; }
        public static string Name { get => name; set => name = value; }
        public static int PacketNumber { get => ++packetNumber == int.MaxValue ? 0 : packetNumber; }
        public static bool ShouldRun { get => shouldRun; set => shouldRun = value; }

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
            ShouldRun = true;

            config.MaximumConnections = 128;
            config.Port = 12345;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            ServerThread.Name = "Server Thread";
            Name = "TODO-Game server";

            Server = new NetServer(config);

            Server.Start();
            ServerThread.Start();
            ServerConsole.Print("The server has started successfully on thread " + ServerThread.ManagedThreadId + "(" + ServerThread.Name + ")", ConsoleFlags.Info);
        }

        private static void Work()
        {
            while(ShouldRun)
            {
                UpdateStats();
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
                            HandleMessages(inc);
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
        }

        private static void UpdateStats()
        {
            ServerConsole.ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ServerConsole.ConsoleWindow.LabelStatsBytesRecieved.Content = "Bytes recieved : " + Server.Statistics.ReceivedBytes + "B";
                ServerConsole.ConsoleWindow.LabelStatsBytesSent.Content = "Bytes sent : " + Server.Statistics.SentBytes + "B";
                ServerConsole.ConsoleWindow.LabelStatsMsgRecieved.Content = "Messages recieved : " + Server.Statistics.ReceivedMessages + "B";
                ServerConsole.ConsoleWindow.LabelStatsMsgSent.Content = "Messages sent : " + Server.Statistics.SentMessages + "B";
            }));
        }

        private static void SendDiscoveryResponse(NetIncomingMessage inc)
        {
            ServerConsole.Print("Recieved discovery request from " + inc.SenderEndPoint.ToString(), ConsoleFlags.Info);
            NetOutgoingMessage msg = Server.CreateMessage();
            msg.Write(Name);

            Server.SendDiscoveryResponse(msg, inc.SenderEndPoint);
        }

        private static void ApproveConnection(NetIncomingMessage inc)
        {
            string secret = inc.ReadString();
            if (secret == "TODO-Game Client")
            {
                inc.SenderConnection.Approve();
                ServerConsole.Print("Approved incoming connection from " + inc.SenderEndPoint.ToString(), ConsoleFlags.Info);
            }
            else
            {
                inc.SenderConnection.Deny();
                ServerConsole.Print("Denied incoming connection from " + inc.SenderEndPoint.ToString() + " (Secret was " + secret + " instead of TODO-Game Client)", ConsoleFlags.Alert);
            }
        }

        private static void HandleMessages(NetIncomingMessage inc)
        {
            NetOutgoingMessage outmsg = Server.CreateMessage();
            ClientMessageTypes MessageType = (ClientMessageTypes)inc.ReadByte();
            switch (MessageType)
            {
                case ClientMessageTypes.SendInitialPlayerInfo:
                    InitialPlayerInfoMessage msg = new InitialPlayerInfoMessage();
                    msg.DecodeMessage(inc);

                    Player p = new Player(msg.ID, msg.TeamNumber, msg.Weapon, msg.Name)
                    { IP = inc.SenderEndPoint.ToString() };

                    bool AlreadyExists = false;

                    foreach (var item in ServerConsole.ConsoleWindow.ListViewPlayerList.Items)
                    {
                        if ((item as Player).ID == p.ID)
                            AlreadyExists = true;
                    }

                    if (!AlreadyExists)
                    {
                        ServerConsole.AddNewPlayerToList(p);

                        ServerConsole.Print("Added player " + p.Name + "(" + p.ID + ") to server list", ConsoleFlags.Debug);
                        ConfirmPlayerArrivalMessage newmsg = new ConfirmPlayerArrivalMessage();
                        newmsg.EncodeMessage(outmsg);
                        Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
