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
        /// Add an item to any chest during World Generation
        /// </summary>
        /// <param name="chestID">ChestID to place items in</param>
        /// <param name="chance">The chance</param>
        /// <param name="itemToPlaceInChest">The item to place in the chests</param>
        /// <param name="min">Minimum amount of items to add. If minimum is higher than maximum then it'll be garanteed amount</param>
        /// <param name="max">Maximum amount of items to add. If maximum is higher than minimum then it'll be a random amount between them</param>
        /// <returns>Number of items, how many times and if it succeeded</returns>
        public static (int, int, bool) AddItemToChest(int chestID, float chance, int itemToPlaceInChest, int min = 1, int max = 0)
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
                                chest.item[inventoryIndex].SetDefaults(itemToPlaceInChest);
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
        /// Add an item to any chest during World Generation
        /// </summary>
        /// <param name="chestID">ChestID to place items in</param>
        /// <param name="chance">The chance</param>
        /// <param name="itemToPlaceInChest">The item to place in the chests</param>
        /// <param name="min">Minimum amount of items to add. If minimum is higher than maximum then it'll be garanteed amount</param>
        /// <param name="max">Maximum amount of items to add. If maximum is higher than minimum then it'll be a random amount between them</param>
        /// <returns>Number of items, how many times and if it succeeded</returns>
        public static (int, int, bool) AddItemToChest(int chestID, int chance, int itemToPlaceInChest, int min = 1, int max = 0)
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
                                chest.item[inventoryIndex].SetDefaults(itemToPlaceInChest);
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
