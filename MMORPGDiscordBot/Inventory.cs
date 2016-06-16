using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMORPGDiscordBot
{
    class Inventory
    {
        //List of items
        public List<ItemObject> inventory = new List<ItemObject>();

        //Gets items based on input
        public ItemObject GetItem(ItemObject item)
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

        //Adds item
        public void AddItem(ItemObject itemToAdd)
        {
            ItemObject newItem = DoesInventoryHaveItem(itemToAdd);
            if (newItem != null)
            {
                newItem.amount++;
            }
            else
            {
                inventory.Add(itemToAdd);
            }
        }

        public ItemObject DoesInventoryHaveItem(ItemObject itemToCheck)
        {
            foreach (var item in inventory)
            {
                if (item == itemToCheck)
                {
                    return item;
                }
            }
            return null;
        }

        //Rmoves item
        public void RemoveItem(ItemObject itemToAdd)
        {
            ItemObject newItem = DoesInventoryHaveItem(itemToAdd);
            if (newItem != null)
            {
                newItem.amount--;
            }
            else
            {
                inventory.Add(itemToAdd);
            }
        }
    }
}
