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
        public String userName { get; private set; }
        public String gender { get; private set;}
        public Place location { get; private set; }
        public Image playerImage { get; set; }
        public Player(String userName, String gender)
        {
            this.userName = userName;
            this.gender = gender;
            location = Place.Town;
            playerImage = null;
        }

        public Image displayPlayer()
        {
            Bitmap bitmap = new Bitmap(playerImage.Width + Location.getLocationImage(location).Width, Math.Max(playerImage.Height, Location.getLocationImage(location).Height));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(playerImage, 0, 0);
                g.DrawImage(Location.getLocationImage(location), playerImage.Width, 0);
            }
            return (Image)bitmap;
        }
    }
}
