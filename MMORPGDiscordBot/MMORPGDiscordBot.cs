using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class MMORPGDiscordBot
    {
        DiscordClient bot;
        
        public MMORPGDiscordBot()
        {
            bot = new DiscordClient();
            bot.Connect("changethis@email.com", "123456789");
            bot.MessageReceived += BotMessageRecieved;
            bot.Wait();
        }

        private void BotMessageRecieved(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "!test")
            {
                e.Channel.SendMessage("test message");
            }
        }
    }
}
