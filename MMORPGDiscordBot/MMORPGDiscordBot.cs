using Discord;
using Newtonsoft.Json;
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
using System.Timers;

namespace MMORPGDiscordBot
{
    class MMORPGDiscordBot
    {
        //Creating the discord bot and the list of players
        DiscordClient bot;
        //List of players
        List<Player> players = new List<Player>();
        //Path to doucments
        string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //Connecting the bot and waiting for messages to be recieved
        public MMORPGDiscordBot()
        {
            //Creating update timer
            System.Timers.Timer timer = new System.Timers.Timer(3000);
            timer.Elapsed += async (sender, e) => HandleTimer();
            timer.Start();

            //Loading player data
            var dirs = Directory.GetDirectories(path + @"\MMORPGDicordBot");
            if (dirs.Length != 0)
            {
                Console.WriteLine(dirs.Length);
                LoadPlayerData();
            }

            //Connecting the bot
            bot = new DiscordClient();
            bot.Connect("mmorpgdiscordbot@gmail.com","");
            bot.MessageReceived += BotMessageRecieved;
            bot.Wait();
        }

        //Updating each player
        private void HandleTimer()
        {
            foreach (Player player in players)
            {
                player.Update();
            }
        }

        //MessageRecieved event
        private void BotMessageRecieved(object sender, MessageEventArgs e)
        {
            if (e.User.Name != "MMORPGBot")
            {
                //Help command
                if (e.Message.Text.Contains("!help"))
                {
                    e.Channel.SendMessage("```MMORPG BOT V0.01\n"
                        + "!create USERNAME GENDER\n"
                        + "!chop USERNAME\n"
                        + "!mine USERNAME\n"
                        + "!display \n"
                        + "!display USERNAME\n"
                        + "```");
                }
                //Create command
                if (e.Message.Text.Contains("!create"))
                {
                    try
                    {
                        var parms = Regex.Split(e.Message.Text.Substring(8), " ");
                        if (parms.Length != 2)
                        {
                            throw new Exception();
                        }
                        if (CheckIfPlayerExist(e.User.Id))
                        {
                            throw new Exception();
                        }
                        CreateNewPlayer(e, parms);
                    }
                    catch (Exception)
                    {
                        e.Channel.SendMessage(e.ToString());
                    }
                }
                //Display command
                if (e.Message.Text == ("!display"))
                {
                    try
                    {
                        DisplayPlayerStats(e);
                    }
                    catch (Exception)
                    {
                        e.Channel.SendMessage("Invalid inputs or this player does not exist");
                    }
                }
                else if (e.Message.Text.Contains("!display"))
                {
                    try
                    {
                        var parms = Regex.Split(e.Message.Text.Substring(9), " ");
                        if (parms.Length == 0)
                        {
                            DisplayPlayerStats(e);
                        }
                        else if (parms.Length == 1)
                        {
                            DisplayPlayerStats(parms[0], e);
                        }
                        else
                        {
                            throw new Exception();
                        }

                    }
                    catch (Exception)
                    {
                        e.Channel.SendMessage("Invalid inputs or this player does not exist");
                    }
                }
                //Mine command
                if (e.Message.Text.Contains("!mine"))
                {
                    try
                    {
                        if (CheckIfPlayerExist(e.User.Id))
                        {
                            GetPlayerById(e.User.Id).action = Action.Mining;
                            GetPlayerById(e.User.Id).location = Place.Mine;
                            Image newImage = GetPlayerById(e.User.Id).DisplayPlayer();
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        e.Channel.SendMessage("Invalid inputs or this player does not exist");
                    }
                }
                //Chop command
                if (e.Message.Text.Contains("!chop"))
                {
                    try
                    {
                        if (CheckIfPlayerExist(e.User.Id))
                        {
                            GetPlayerById(e.User.Id).action = Action.WoodCutting;
                            GetPlayerById(e.User.Id).location = Place.Forest;
                            Image newImage = GetPlayerById(e.User.Id).DisplayPlayer();
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        e.Channel.SendMessage("Invalid inputs or this player does not exist");
                    }
                }
                //Move command
                if (e.Message.Text.Contains("!move"))
                {
                    try
                    {
                        var parms = Regex.Split(e.Message.Text.Substring(6), " ");
                        if (parms.Length == 1)
                        {
                            if (CheckIfPlayerExist(e.User.Id))
                            {
                                if (parms[0].ToLower().Contains("forest"))
                                {
                                    GetPlayerById(e.User.Id).action = Action.Nothing;
                                    GetPlayerById(e.User.Id).location = Place.Forest;
                                }
                                else if (parms[0].ToLower().Contains("mine"))
                                {
                                    GetPlayerById(e.User.Id).action = Action.Nothing;
                                    GetPlayerById(e.User.Id).location = Place.Mine;
                                }
                                else if (parms[0].ToLower().Contains("town"))
                                {
                                    GetPlayerById(e.User.Id).action = Action.Nothing;
                                    GetPlayerById(e.User.Id).location = Place.Town;
                                }
                                Image newImage = GetPlayerById(e.User.Id).DisplayPlayer();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                if (e.Message.Text.Contains("!attack"))
                {
                    try
                    {
                        var parms = Regex.Split(e.Message.Text.Substring(6), " ");
                        if (parms.Length == 1)
                        {
                            if(GetPlayerByUserName(parms[0]) != null)
                            {
                                //TODO: Deal damage to players
                                e.Channel.SendMessage(GetPlayerById(e.User.Id).userName + " attacks " + GetPlayerByUserName(parms[0]).userName);
                            }
                            else
                            {
                                e.Channel.SendMessage("This player does not exist!");
                            }
                        }
                    }
                    catch(Exception)
                    {
                        e.Channel.SendMessage("Invalid inputs.");
                    }
            }
        }

        //Creates the new player
        private void CreateNewPlayer(MessageEventArgs e,String[] parms)
        {
            //Username and gender are entered and then the default player pictured is created
            try
            {
                string userName = parms[0];
                string gender = parms[1];
                Player newPlayer = new Player(userName, gender, Place.Town, null, 0, 0,Action.Nothing,e.User.Id, true);
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
        
        //Load player data
        private void LoadPlayerData()
        {
            String[] dirs = Directory.GetDirectories(path + @"\MMORPGDicordBot");
            Player newPlayer;
            Inventory playerInventory = new Inventory();
            Image playerImage = null;
            JToken jToken;
            string userName = "";
            string gender = "";
            string location = "";
            string woodCutting = "";
            string mining = "";
            string action = "";
            string id = "";
            foreach (var dir in dirs)
            {
                String[] files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    if (file.Contains("Picture"))
                    {
                        playerImage = Image.FromFile(file);
                        playerImage.Dispose();
                    }
                    else if (file.Contains("player.json"))
                    {
                        string text = System.IO.File.ReadAllText(file);
                        jToken = JsonConvert.DeserializeObject<JToken>(text);
                        userName = jToken.SelectToken("userName").ToString();
                        gender = jToken.SelectToken("gender").ToString();
                        location = jToken.SelectToken("location").ToString();
                        woodCutting = jToken.SelectToken("woodCutting").ToString();
                        mining = jToken.SelectToken("mining").ToString();
                        action = jToken.SelectToken("action").ToString();
                        id = jToken.SelectToken("id").ToString();

                    }
                    else if (files.Contains("inventory"))
                    {
                        jToken = JObject.Parse(file);
                        JArray jArray = JArray.FromObject(jToken);
                        foreach (JObject content in jArray)
                        {
                            foreach (JProperty item in content.Properties())
                            {
                                playerInventory.AddItem(new ItemObject(ItemObject.GetItemTypeByString(item.Name.ToString()), (int)item.Value));
                            }
                        }
                    }
                }
                newPlayer = new Player(userName,gender,Location.getLocationByString(location),playerImage,(float)Convert.ToDecimal(woodCutting),(float)Convert.ToDecimal(mining),ActionHelper.GetActionByString(action),(ulong)Convert.ToInt64(id));
                newPlayer.inventory = playerInventory;
                players.Add(newPlayer);
            }
        }

        //Display player stats for single user
        private void DisplayPlayerStats(MessageEventArgs e)
        {
            Player player = GetPlayerById(e.User.Id);
            e.Channel.SendMessage("```stats: \n"
                       + "UserName: " + player.userName + "\n"
                       + "Gender: " + player.gender + "\n"
                       + "Location: " + player.location + "\n"
                       + "Woodcutting: " + player.woodCutting + "\n"
                       + "Mining: " + player.mining + "\n"
                       + "```");
            e.Channel.SendFile(path + @"\MMORPGDicordBot\" + GetPlayerById(e.User.Id).userName + @"\" + "PlayerPicture.png");
        }
        //Display player stats for another user
        private void DisplayPlayerStats(string userName, MessageEventArgs e)
        {
            Player player = GetPlayerByUserName(userName);
            e.Channel.SendMessage("```stats: \n"
                       + "UserName: " + player.userName + "\n"
                       + "Gender: " + player.gender + "\n"
                       + "Location: " + player.location + "\n"
                       + "Woodcutting: " + player.woodCutting + "\n"
                       + "Mining: " + player.mining + "\n"
                       + "```");
            e.Channel.SendFile(path + @"\MMORPGDicordBot\" + userName + @"\" + "PlayerPicture.png");
        }

        //Get player by user name
        private Player GetPlayerByUserName(string userName)
        {
            foreach (Player player in players)
            {
                if (player.userName == userName)
                {
                    return player;
                }
            }
            return null;
        }

        //Get player by id
        private Player GetPlayerById(ulong id)
        {
            foreach (Player player in players)
            {
                if(player.id == id)
                {
                    return player;
                }
            }
            return null;
        }

        //A function to check if a player already exists
        private Boolean CheckIfPlayerExist(ulong id)
        {
            foreach (Player player in players)
            {
                if (player.id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
