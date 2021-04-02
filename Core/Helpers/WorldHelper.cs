using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static class WorldHelper
    {
        public static void UpdateWorldBool()
        {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static (int, int, bool) AddItemToChest(int chestID, float chance, int itemsToPlaceInChest, int min = 1, int max = 0)
        {
            int howManyItems = 0;
            int howManyTimes = 0;
            bool addedItemAtleastOnce = false;
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == chestID * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        int itemAmount;
                        if (max < min)
                            itemAmount = min;
                        else
                            itemAmount = WorldGen.genRand.Next(min, max + 1);
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextFloat() < chance)
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInChest);
                                chest.item[inventoryIndex].stack = itemAmount;
                                howManyItems += itemAmount;
                                howManyTimes++;
                                addedItemAtleastOnce = true;
                            }
                            break;
                        }
                    }
                }
            }

            return (howManyItems, howManyTimes, addedItemAtleastOnce);
        }

        /// <summary>
        /// Adds items to a chest
        /// </summary>
        /// <param name="chestID"></param>
        /// <param name="chance"></param>
        /// <param name="itemsToPlaceInChest"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Number of items, how many times and if it succeeded</returns>
        public static (int, int, bool) AddItemToChest(int chestID, int chance, int itemsToPlaceInChest, int min = 1, int max = 0)
        {
            int howManyItems = 0;
            int howManyTimes = 0;
            bool addedItemAtleastOnce = false;
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == chestID * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        int itemAmount;
                        if (max < min)
                            itemAmount = min;
                        else
                            itemAmount = WorldGen.genRand.Next(min, max + 1);
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.Next(chance) == 0)
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInChest);
                                chest.item[inventoryIndex].stack = itemAmount;
                                howManyItems += itemAmount;
                                howManyTimes++;
                                addedItemAtleastOnce = true;
                            }
                            break;
                        }
                    }
                }
            }

            return (howManyItems, howManyTimes, addedItemAtleastOnce);
        }
    }
}
