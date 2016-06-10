using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MMORPGDiscordBot
{
    class Player
    {
        //Username(Decided by the user)
        public String userName {get; private set;}
        //Gender(Decided by the user)
        public String gender {get; private set;}
        //Location the player is in
        public Place location {get; private set;}
        //The users player image
        public Image playerImage {get; set;}
        //The users inventory
        public Inventory inventory {get; private set;}
        //Players statics
        public Stats stats {get; private set;}
        
        //Default constructor
        public Player(String userName, String gender)
        {
            this.userName = userName;
            this.gender = gender;
            location = Place.Town;
            playerImage = null;
            stats.woodCutting = 0;
            stats.mining = 0;
        }
        //Displays the player by command the player's image and their location
        public Bitmap DisplayPlayer()
        {
            Bitmap bitmap = new Bitmap(playerImage.Width + Location.GetLocationImage(location).Width, Math.Max(playerImage.Height, Location.GetLocationImage(location).Height));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(playerImage, 0, 0);
                g.DrawImage(Location.GetLocationImage(location), playerImage.Width, 0);
            }
            return bitmap;
        }
        private void CreatePlayerJSON()
        {
                
        }
    }
}
