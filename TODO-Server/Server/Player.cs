using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Server
{
    /// <summary>
    /// Describes a player present on the server
    /// </summary>
    public class Player
    {
        #region Fields

        private long id;
        private int hp;
        private string name;
        private string weapon;
        private NetConnection connection;
        private float mouseRotationAngle;
        private int team;
        private int ping;
        private string ip;

        #endregion

        #region Properties

        /// <summary>
        /// The player's unique ID
        /// </summary>
        public long ID { get => id; set => id = value; }

        /// <summary>
        /// The player's Health Points (HP)
        /// </summary>
        public int HP { get => hp; set => hp = value; }

        /// <summary>
        /// The player's name
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// The player's weapon
        /// </summary>
        public string Weapon { get => weapon; set => weapon = value; }

        /// <summary>
        /// The player's associated connection
        /// </summary>
        public NetConnection Connection { get => connection; set => connection = value; }

        /// <summary>
        /// The player's mouse rotation angle (used for displaying it's rotation)
        /// </summary>
        public float MouseRotationAngle { get => mouseRotationAngle; set => mouseRotationAngle = value; }

        /// <summary>
        /// The player's team
        /// </summary>
        public int Team { get => team; set => team = value; }

        /// <summary>
        /// Describes wether or not the player is alive
        /// </summary>
        public bool IsAlive { get => HP <= 0; }

        /// <summary>
        /// The last ping registered for the player
        /// </summary>
        public int Ping { get => ping; set => ping = value; }

        /// <summary>
        /// The player's associated Internet Protocol Adress v4 (IP)
        /// </summary>
        public string IP { get => ip; set => ip = value; }

        #endregion

        #region Constructors

        public Player(long id)
        {
            ID = id;
            Ping = 0;
        }

        public Player(long id, int team)
            : this (id)
        {
            Team = team;
        }

        public Player(long id, int team, string weapon, string name)
            : this(id, team)
        {
            Weapon = weapon;
            Name = name;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name + " (" + ID + ")";
        }

        #endregion
    }
}
