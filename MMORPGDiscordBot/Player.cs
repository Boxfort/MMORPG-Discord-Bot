using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

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
        public Inventory inventory {get; set;}
        //Woodcutting
        public float woodCutting {get; private set;}
        //Mining
        public float mining {get; private set;}

        
        //Default constructor
        public Player(string userName, string gender,Place location,Image playerImage, float woodCutting, float mining,bool newPlayer)
        {
            this.userName = userName;
            this.gender = gender;
            location = Place.Town;
            playerImage = null;
            inventory = new Inventory();
            woodCutting = 0;
            mining = 0;
            CreatePlayerJSON();
        }
        public Player(string userName, string gender, Place location, Image playerImage, float woodCutting, float mining)
        {
            this.userName = userName;
            this.gender = gender;
            this.location = location;
            inventory = new Inventory();
            this.playerImage = playerImage;
            this.woodCutting = woodCutting;
            this.mining = mining;
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
            inventory.AddItem(new ItemObject(Item.Wood, 10));
            playerDic.Add("userName", userName);
            playerDic.Add("gender", gender);
            playerDic.Add("location", location.ToString());
            playerDic.Add("woodCutting", woodCutting.ToString());
            playerDic.Add("mining", mining.ToString());
            foreach (ItemObject item in inventory.inventory)
            {
                inventoryDic.Add(item.item.ToString(), item.amount.ToString());
            }
            Console.WriteLine(inventoryDic.Count);
            String playerJson = JsonConvert.SerializeObject(playerDic);
            String inventoryJson = JsonConvert.SerializeObject(inventoryDic);
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(path + @"\MMORPGDicordBot\" + userName);
            System.IO.File.WriteAllText(path + @"\MMORPGDicordBot\" + userName + @"\" + "player.json", playerJson);
            System.IO.File.WriteAllText(path + @"\MMORPGDicordBot\" + userName + @"\" + "inventory.json", inventoryJson);
        }
    }
}
