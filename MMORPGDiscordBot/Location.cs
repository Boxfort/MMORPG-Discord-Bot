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
        //Three location in game
        public static Bitmap townLocation { get; private set; }
        public static Bitmap forestLocation { get; private set; }
        public static Bitmap mineLocation { get; private set; }

        //Sets location when first instance of class is created
        static Location()
        {
            townLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.town.png"));
            forestLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.forest.png"));
            mineLocation = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.mine.png"));
        }

        //Return the location image based on place input
        public static Bitmap GetLocationImage(Place place)
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

        public static Place getLocationByString(string input)
        {
            if (input.Contains("town"))
            {
                return Place.Town;
            }
            else if (input.Contains("forest"))
            {
                return Place.Forest;
            }
            else if (input.Contains("mine"))
            {
                return Place.Mine;
            }
            return Place.Town;
        }
    }
}
