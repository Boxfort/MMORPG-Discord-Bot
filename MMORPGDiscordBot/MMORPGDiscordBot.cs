using Discord;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class MMORPGDiscordBot
    {
        DiscordClient bot;
        List<Player> players = new List<Player>();
        public MMORPGDiscordBot()
        {
            bot = new DiscordClient();
            bot.Connect("mmorpgdiscordbot@gmail.com", "asdf12345");
            bot.MessageReceived += BotMessageRecieved;
            bot.Wait();
        }

        private void BotMessageRecieved(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "!help")
            {
                e.Channel.SendMessage("Use !Create USERNAME GENDER then attach a file with your player picture");
            }
            else if(e.Message.Text.Contains("!Create"))
            {
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

        private void CreateNewPlayer(MessageEventArgs e,String[] parms)
        {
            try
            {
                String userName = parms[0];
                String gender = parms[1];
                Player newPlayer = new Player(userName, gender);
                players.Add(newPlayer);
                e.Channel.SendMessage("Player " + userName + " entered the world");
                newPlayer.playerImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MMORPGDiscordBot.DefaultPlayer.png"));
                Image playerImageLoad = newPlayer.displayPlayer();
                playerImageLoad.Save("C:\\MyFile2.png");
                e.Channel.SendFile("C:\\MyFile2.png");


            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            

        }
    }
}
