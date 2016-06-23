using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Reflection;

namespace MMORPGDiscordBot
{
    class Player
    {
        //Username(Decided by the user)
        public String userName {get; private set;}
        //Gender(Decided by the user)
        public String gender {get; private set;}
        //Location the player is in
        public Place location {get; set;}
        //The users player image
        public Image playerImage {get; set;}
        //The users inventory
        public Inventory inventory {get; set;}
        //Woodcutting
        public float woodCutting {get; private set;}
        public int woodCuttingLevel { get; private set; }
        //Mining
        public float mining {get; private set;}
        public int miningLevel { get; private set; }
        //Player action
        public Action action { get; set; }
        //Player ID
        public ulong id { get; set; }

        
        //Default constructor
        public Player(string userName, string gender,Place location,Image playerImage, float woodCutting, float mining, Action action, ulong playerID, bool newPlayer)
        {
            this.userName = userName;
            this.gender = gender;
            location = Place.Town;
            playerImage = null;
            inventory = new Inventory();
            woodCutting = 0;
            mining = 0;
            this.action = action;
            this.id = playerID;
            CreatePlayerJSON();
        }
        //Default constructor for non newPlayers
        public Player(string userName, string gender, Place location, Image playerImage, float woodCutting, float mining, Action action, ulong playerID)
        {
            this.userName = userName;
            this.gender = gender;
            this.location = location;
            inventory = new Inventory();
            this.playerImage = playerImage;
            this.woodCutting = woodCutting;
            this.mining = mining;
            this.action = action;
            this.action = action;
            this.id = playerID;
        }

        //Updates the player action
        public void Update()
        {
            if (action == Action.Mining)
            {
                mining += 1f;
            }
            else if (action == Action.WoodCutting)
            {
                woodCutting += 1f;
            }
            CreatePlayerJSON();
            Console.WriteLine("updated");
        }

        //Displays the player by command the player's image and their location
        public Bitmap DisplayPlayer()
        {
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string outputFileName = "PlayerPicture.png";
                playerImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.DefaultPlayer.png"));
                Bitmap bitmap = new Bitmap(playerImage.Width + Location.GetLocationImage(location).Width, Math.Max(playerImage.Height, Location.GetLocationImage(location).Height));
                Graphics g;
                Console.WriteLine("made it");
                using (g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(playerImage, 0, 0);
                    g.DrawImage(Location.GetLocationImage(location), playerImage.Width, 0);
                }
                playerImage.Dispose();
                File.Delete(path + @"\MMORPGDicordBot\" + userName + @"\" + outputFileName);
                bitmap.Save(path + @"\MMORPGDicordBot\" + userName + @"\" + outputFileName);
                return bitmap;
            }
            
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

        }

        //Create player data
        public void CreatePlayerJSON()
        {
            Dictionary<string, string> playerDic = new Dictionary<string, string>();
            Dictionary<string, string> inventoryDic = new Dictionary<string, string>();
            inventory.AddItem(new ItemObject(Item.Wood, 10));
            playerDic.Add("userName", userName);
            playerDic.Add("gender", gender);
            playerDic.Add("location", location.ToString());
            playerDic.Add("woodCutting", woodCutting.ToString());
            playerDic.Add("mining", mining.ToString());
            playerDic.Add("action", action.ToString());
            playerDic.Add("playerId", id.ToString());
            inventory.inventory.Clear();
            foreach (ItemObject item in inventory.inventory)
            {
                    inventoryDic.Add(item.item.ToString(), item.amount.ToString());
            }
            Console.WriteLine(inventoryDic.Count);
            String playerJson = JsonConvert.SerializeObject(playerDic,Formatting.Indented);
            String inventoryJson = JsonConvert.SerializeObject(inventoryDic, Formatting.Indented);
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(path + @"\MMORPGDicordBot\" + userName);
            System.IO.File.WriteAllText(path + @"\MMORPGDicordBot\" + userName + @"\" + "player.json", playerJson);
            System.IO.File.WriteAllText(path + @"\MMORPGDicordBot\" + userName + @"\" + "inventory.json", inventoryJson);
        }
    }
}
