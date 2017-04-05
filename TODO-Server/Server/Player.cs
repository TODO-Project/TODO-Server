using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_Server.Server
{
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

        public long ID { get => id; set => id = value; }
        public int HP { get => hp; set => hp = value; }
        public string Name { get => name; set => name = value; }
        public string Weapon { get => weapon; set => weapon = value; }
        public NetConnection Connection { get => connection; set => connection = value; }
        public float MouseRotationAngle { get => mouseRotationAngle; set => mouseRotationAngle = value; }
        public int Team { get => team; set => team = value; }
        public bool IsAlive { get => HP <= 0; }
        public int Ping { get => ping; set => ping = value; }
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

        #endregion
    }
}
