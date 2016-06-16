using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class MMORPGDiscordBot
    {
        //Creating the discord bot and the list of players
        DiscordClient bot;
        List<Player> players = new List<Player>();
        string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //Connecting the bot and waiting for messages to be recieved
        public MMORPGDiscordBot()
        {
            String[] dirs = Directory.GetDirectories(path + @"\MMORPGDicordBot");
            if(dirs.Length != 0)
            {
                Console.WriteLine(dirs.Length);
                LoadPlayerData();
            }
            bot = new DiscordClient();
            bot.Connect("mmorpgdiscordbot@gmail.com", "");
            bot.MessageReceived += BotMessageRecieved;
            bot.Wait();
        }
        //MessageRecieved event
        private void BotMessageRecieved(object sender, MessageEventArgs e)
        {
            //Help command
            if (e.Message.Text == "!help")
            {
                e.Channel.SendMessage("Use !Create USERNAME GENDER then attach a file with your player picture");
            }
            //Create command
            else if(e.Message.Text.Contains("!Create"))
            {
                //Get the the userName and gender
                try
                {
                    string[] parms = Regex.Split(e.Message.Text.Substring(8), " ");
                    if(parms.Length != 2)
                    {
                        throw new Exception();
                    }
                    CreateNewPlayer(e, parms);
                }
                catch
                {
                    e.Channel.SendMessage("Invalid inputs");
                }
            }
        }
        //Creates the new player
        private void CreateNewPlayer(MessageEventArgs e,String[] parms)
        {
            //Username and gender are entered and then the default player pictured is created
            try
            {
                String userName = parms[0];
                String gender = parms[1];
                Player newPlayer = new Player(userName, gender, Place.Town, null, 0, 0,true);
                players.Add(newPlayer);
                e.Channel.SendMessage("Player " + userName + " entered the world");
                newPlayer.playerImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.DefaultPlayer.png"));
                Image playerImageLoad = newPlayer.DisplayPlayer();
                if (!Directory.Exists(path + @"\MMORPGDicordBot"))
                {
                    Directory.CreateDirectory(path + @"\MMORPGDicordBot");
                }
                Directory.CreateDirectory(path + @"\MMORPGDicordBot\" + userName);
                string outputFileName = "PlayerPicture.png";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(path + @"\MMORPGDicordBot\" + userName + @"\" + outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        playerImageLoad.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                e.Channel.SendFile(path + @"\MMORPGDicordBot\" + userName + @"\" + outputFileName);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void LoadPlayerData()
        {
            String[] dirs = Directory.GetDirectories(path + @"\MMORPGDicordBot");
            
            Player newPlayer;
            Inventory playerInventory = new Inventory();
            Image playerImage = null;
            JObject jObject;
            string userName = "";
            string gender = "";
            string location = "";
            string woodCutting = "";
            string mining = "";
            foreach (var dir in dirs)
            {
                String[] files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    if (file.Contains("Picture"))
                    {
                        playerImage = Image.FromFile(file);
                    }
                    else if (file.Contains("player.txt"))
                    {
                        Console.WriteLine(file);
                        jObject = JObject.Parse(file);
                        userName = jObject["userName"].ToString();
                        gender = jObject["gender"].ToString();
                        location = jObject["location"].ToString();
                        woodCutting = jObject["woodCutting"].ToString();
                        mining = jObject["mining"].ToString();
                    }
                    else if (files.Contains("inventory"))
                    {
                        jObject = JObject.Parse(file);
                        JArray jArray = JArray.FromObject(jObject);
                        foreach (JObject content in jArray)
                        {
                            foreach (JProperty item in content.Properties())
                            {
                                playerInventory.AddItem(new ItemObject(ItemObject.GetItemTypeByString(item.Name.ToString()), (int)item.Value));
                            }
                        }
                    }
                }
                newPlayer = new Player(userName,gender,Location.getLocationByString(location),playerImage,0,0);
                newPlayer.inventory = playerInventory;
            }
        }
    }
}
