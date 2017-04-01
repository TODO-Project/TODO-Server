using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO_Server.Console;

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

        #endregion

        #region Properties

        public static List<Player> PlayerList { get => playerList; set => playerList = value; }
        public static NetServer Server { get => server; private set => server = value; }
        public static int Seed { get => seed; set => seed = value; }

        #endregion

        #region Methods

        public static void Initialize()
        {
            ServerConsole.Print("Initializing game server...", ConsoleFlags.Info);
            Seed = Guid.NewGuid().GetHashCode();
            ServerConsole.Print("Seed generated : " + Seed, ConsoleFlags.Info);
        }

        #endregion

    }
}
