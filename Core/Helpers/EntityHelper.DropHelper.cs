using KawaggyMod.Core.Structs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static partial class EntityHelper
    {
        /// <returns>Amount of items dropped</returns>
        public static int DropItem(this Entity entity, ItemDropInfo item)
        {
            if (item.type <= 0)
                return 0;

            int itemStack;
            if (item.max <= item.min)
                itemStack = item.min;
            else
                itemStack = Main.rand.Next(item.min, item.max + 1);

            if (itemStack <= 0)
                return 0;

            if (item.dropPerPlayer)
                entity.DropItemInstanced(item.type, itemStack);
            else
                Item.NewItem(entity.Hitbox, item.type, itemStack);

            return itemStack;
        }

        
        /// <returns>A dictionary containing every item type and its amount dropped</returns>
        public static Dictionary<int, int> DropItems(this Entity entity, ItemDropInfo[] items)
        {
            Dictionary<int, int> collection = new Dictionary<int, int>();

            for (int i = 0; i < items.Length; i++)
            {
                int amount = entity.DropItem(items[i]);
                collection.Add(items[i].type, amount);
            }

            return collection;
        }

        public static void DropItemInstanced(Vector2 position, Vector2 hitbox, int itemType, int itemStack)
        {
            if (itemType <= 0)
                return;

            if (Main.netMode == NetmodeID.Server)
            {
                int theItem = Item.NewItem((int)position.X, (int)position.Y, (int)hitbox.X, (int)hitbox.Y, itemType, itemStack, noBroadcast: true);
                Main.itemLockoutTime[theItem] = 54000;

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        NetMessage.SendData(MessageID.InstancedItem, i, -1, null, theItem);
                    }
                }

                Main.item[theItem].active = false;
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Item.NewItem((int)position.X, (int)position.Y, (int)hitbox.X, (int)hitbox.Y, itemType, itemStack);
            }
        }

        public static void DropItemInstanced(this Entity entity, int itemType, int itemStack)
        {
            DropItemInstanced(entity.position, new Vector2(entity.Hitbox.Width, entity.Hitbox.Height), itemType, itemStack);
        }
    }
}
