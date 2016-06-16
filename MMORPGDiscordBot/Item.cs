using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    //Item types
    enum Item
    {
        Wood,
        Ore,
        Gold,
        Null
    }
    //Item object
    class ItemObject
    {
        public ItemObject(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public Item item { get; private set; }
        public int amount { get; set; }

        public static Item GetItemTypeByString(string input)
        {
            if (input.Contains("Wood"))
            {
                return Item.Wood;
            }
            else if (input.Contains("Ore"))
            {
                return Item.Ore;
            }
            else if (input.Contains("Gold"))
            {
                return Item.Gold;
            }
            return Item.Null;
        }
    }
}
