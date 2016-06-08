using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class Player
    {
        public String userName { get; private set; }
        public String gender { get; private set;}
        public Place location { get; private set; }
        public Player(String userName, String gender)
        {
            this.userName = userName;
            this.gender = gender;
        }
    }
}
