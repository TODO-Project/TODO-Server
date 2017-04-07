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
    /// <summary>
    /// Describes the game server, handling incoming and outcoming messages,
    /// and interfacing with the user.
    /// </summary>
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
        
        /// <summary>
        /// The list of players present on the server
        /// </summary>
        public static List<Player> PlayerList { get => playerList; set => playerList = value; }

        /// <summary>
        /// The server handling connections and messages
        /// </summary>
        public static NetServer Server { get => server; private set => server = value; }

        /// <summary>
        /// The randomly generated seed used for map generation
        /// </summary>
        public static int Seed { get => seed; set => seed = value; }

        /// <summary>
        /// The working thread, handling messages
        /// </summary>
        public static Thread ServerThread { get => serverThread; set => serverThread = value; }

        /// <summary>
        /// The name used by the server to identify incoming connections from clients
        /// </summary>
        public static string Name { get => name; set => name = value; }

        /// <summary>
        /// Unusued. Gets the next packet number
        /// </summary>
        public static int PacketNumber { get => ++packetNumber == int.MaxValue ? 0 : packetNumber; }

        /// <summary>
        /// Describes whether or not the server should continue to run
        /// </summary>
        public static bool ShouldRun { get => shouldRun; set => shouldRun = value; }

        #endregion

        #region Methods

        #region Initialization

        /// <summary>
        /// Initializes the server
        /// </summary>
        public static void Initialize()
        {
            InitializeSeed();
            InitializeServer();
        }

        /// <summary>
        /// Initializes the seed for the server
        /// </summary>
        private static void InitializeSeed()
        {
            ServerConsole.Print("Initializing game server...", ConsoleFlags.Info);
            Seed = Guid.NewGuid().GetHashCode();
            ServerConsole.Print("Seed generated : " + Seed, ConsoleFlags.Info);
        }

        /// <summary>
        /// Initializes the server values
        /// </summary>
        private static void InitializeServer()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("TODO-Game");
            ServerThread = new Thread(Work);
            ShouldRun = true;
            PlayerList = new List<Player>();
            ServerConsole.ConsoleWindow.ListViewPlayerList.ItemsSource = PlayerList;

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

        #endregion

        #region Work

        /// <summary>
        /// The method used by the working thread : handles messages
        /// </summary>
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

        #endregion

        #region Interface

        /// <summary>
        /// Updates the server statistics
        /// </summary>
        private static void UpdateStats()
        {
            ServerConsole.ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ServerConsole.ConsoleWindow.LabelStatsBytesRecieved.Content = "Bytes recieved : " + Server.Statistics.ReceivedBytes + "B";
                ServerConsole.ConsoleWindow.LabelStatsBytesSent.Content = "Bytes sent : " + Server.Statistics.SentBytes + "B";
                ServerConsole.ConsoleWindow.LabelStatsMsgRecieved.Content = "Messages recieved : " + Server.Statistics.ReceivedMessages;
                ServerConsole.ConsoleWindow.LabelStatsMsgSent.Content = "Messages sent : " + Server.Statistics.SentMessages;
            }));
        }

        /// <summary>
        /// Refreshes the player list
        /// </summary>
        private static void RefreshPlayerList()
        {
            ServerConsole.ConsoleWindow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ServerConsole.ConsoleWindow.ListViewPlayerList.Items.Refresh();
            }));
        }

        #endregion

        #region Messages

        /// <summary>
        /// Handles data messages sent by the client
        /// </summary>
        /// <param name="inc">The incoming message</param>
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

                    RefreshPlayerList();

                    if (PlayerList.Find(x => x.ID == p.ID) == null)
                    {
                        PlayerList.Add(p);
                        ConfirmPlayerArrivalMessage newmsg = new ConfirmPlayerArrivalMessage();
                        newmsg.EncodeMessage(outmsg);
                        Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sends a discovery response message to the sender
        /// </summary>
        /// <param name="inc">The incoming message</param>
        private static void SendDiscoveryResponse(NetIncomingMessage inc)
        {
            ServerConsole.Print("Recieved discovery request from " + inc.SenderEndPoint.ToString(), ConsoleFlags.Info);
            NetOutgoingMessage msg = Server.CreateMessage();
            msg.Write(Name);

            Server.SendDiscoveryResponse(msg, inc.SenderEndPoint);
        }

        /// <summary>
        /// Approves the connection from a sender
        /// </summary>
        /// <param name="inc">The incoming connection message</param>
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

        /// <summary>
        /// Disconnects a player using its name and a flag describing the disconnection context
        /// </summary>
        /// <param name="flag">The disconnection flag</param>
        /// <param name="name">The name of the player to be kicked</param>
        /// <returns>True if the player has been kicked; false otherwise</returns>
        public static bool DisconnectPlayer(DisconnectionFlags flag, string name)
        {
            Player p = PlayerList.Find(x => x.Name == name);
            if (p == null)
                return false;
            DisconnectedFromServerMessage msg = new DisconnectedFromServerMessage(flag, p.ID);
            foreach (var connection in Server.Connections)
            {
                NetOutgoingMessage outmsg = Server.CreateMessage();
                msg.EncodeMessage(outmsg);
                Server.SendMessage(outmsg, connection, NetDeliveryMethod.ReliableOrdered);
            }
            PlayerList.Remove(p);
            RefreshPlayerList();
            return true;
        }

        #endregion

        #endregion

    }
}
