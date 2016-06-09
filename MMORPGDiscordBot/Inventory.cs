using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class Inventory
    {
        public List<ItemObject> inventory = new List<ItemObject>();

        public ItemObject getItem(ItemObject item)
        {
            foreach (var itemFound in inventory)
            {
                if (itemFound == item)
                {
                    return itemFound;
                }
            }
            return null;
        }

        public void addItem(ItemObject itemToAdd)
        {
            foreach (var itemFound in inventory)
            {
                if (itemFound == itemToAdd)
                {
                    itemFound.amount += itemToAdd.amount;
                }
            }
        }

        public void removeItem(ItemObject itemToAdd)
        {
            foreach (var itemFound in inventory)
            {
                if (itemFound == itemToAdd)
                {
                    itemFound.amount -= itemToAdd.amount;
                }
            }
        }
    }
}
