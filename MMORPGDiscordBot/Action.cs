using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    //Actions the player can perform
    enum Action
    {
        WoodCutting,
        Mining,
        Nothing
    }

    static class ActionHelper
    {
        public static Action GetActionByString(string actionString)
        {
            if(actionString.ToLower().Contains("wood"))
            {
                return Action.WoodCutting;
            }
            else if (actionString.ToLower().Contains("mine"))
            {
                return Action.Mining;
            }
            else
            {
                return Action.Nothing;
            }
        }
    }
}
