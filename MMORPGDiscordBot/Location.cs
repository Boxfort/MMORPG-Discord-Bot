using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    enum Place
    {
        Town,
        Forest,
        Mine
    };

    static class Location
    {
        public static Bitmap townLocation { get; private set; }
        public static Bitmap forestLocation { get; private set; }
        public static Bitmap mineLocation { get; private set; }

        static Location()
        {
            townLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.town.png"));
            forestLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.forest.png"));
            //Mine is broken for some reason
            //mineLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.mine.png"));
        }

        public static Bitmap getLocationImage(Place place)
        {
            if (place == Place.Town)
            {
                return townLocation; 
            }
            else if (place == Place.Forest)
            {
                return forestLocation;
            }
            else if (place == Place.Mine)
            {
                return mineLocation;
            }
            return null;
        }
    }
}
