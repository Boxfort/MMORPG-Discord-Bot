﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

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
        //Woodcutting
        public float woodCutting {get; private set;}
        //Mining
        public float mining {get; private set;}

        
        //Default constructor
        public Player(String userName, String gender)
        {
            this.userName = userName;
            this.gender = gender;
            location = Place.Town;
            playerImage = null;
            woodCutting = 0;
            mining = 0;
            CreatePlayerJSON();
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
            Dictionary<string, string> playerDic = new Dictionary<string, string>();
            Dictionary<string, string> inventoryDic = new Dictionary<string, string>();
            playerDic.Add("userName", userName);
            playerDic.Add("gender", gender);
            playerDic.Add("location", location.ToString());
            playerDic.Add("woodCutting", woodCutting.ToString());
            playerDic.Add("mining", mining.ToString());
            String playerJson = JsonConvert.SerializeObject(playerDic);
            String inventoryJson = JsonConvert.SerializeObject(inventoryDic);
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.File.WriteAllText(path + @"\MMMORPGDicordBot\" + userName + @"\" + "player.txt", playerJson);
            System.IO.File.WriteAllText(path + @"\MMMORPGDicordBot\" + userName + @"\" + "inventory.txt", inventoryJson);
        }
    }
}
