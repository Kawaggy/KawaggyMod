using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static class DropHelper
    {
        public static int DropItem(this Entity entity, int itemID, bool dropPerPlayer, int min = 1, int max = 0)
        {
            int numberOfItems;
            if (max <= min)
                numberOfItems = min;
            else
                numberOfItems = Main.rand.Next(min, max + 1);

            if (numberOfItems <= 0)
                return 0;

            if (dropPerPlayer)
            {
                entity.DropItemInstanced(itemID, numberOfItems);
                return numberOfItems;
            }


            Item.NewItem(entity.Hitbox, itemID, numberOfItems);
            return numberOfItems;
        }

        public static int DropItem(this Entity entity, int itemID, int min = 1, int max = 0) 
            => DropItem(entity, itemID, false, min, max);

        public static int ChanceDropItem(this Entity entity, int itemID, float chance, bool dropPerPlayer, int min = 1, int max = 0)
        {
            if (Main.rand.NextFloat() < chance)
                return DropItem(entity, itemID, dropPerPlayer, min, max);
            return 0;
        }

        public static int ChanceDropItem(this Entity entity, int itemID, float chance, int min = 1, int max = 0)
            => ChanceDropItem(entity, itemID, chance, false, min, max);

        public static int ConditionalDropItem(this Entity entity, int itemID, bool condition, bool dropPerPlayer, int min = 1, int max = 0)
        {
            if (condition)
                return DropItem(entity, itemID, dropPerPlayer, min, max);
            return 0;
        }

        public static int ConditionalDropItem(this Entity entity, int itemID, bool condition, int min = 1, int max = 0)
            => ConditionalDropItem(entity, itemID, condition, false, min, max);

        public static int ConditionalChanceDropItem(this Entity entity, int itemID, bool condition, float chance, bool dropPerPlayer, int min = 1, int max = 0)
        {
            if (condition)
                return ChanceDropItem(entity, itemID, chance, dropPerPlayer, min, max);
            return 0;
        }

        public static int ConditionalChanceDropItem(this Entity entity, int itemID, bool condition, float chance, int min = 1, int max = 0)
            => ConditionalChanceDropItem(entity, itemID, condition, chance, false, min, max);

        public static void DropItemInstanced(this Entity entity, int itemType, int itemStack = 1)
        {
            if (itemType <= 0)
                return;

            if (Main.netMode == NetmodeID.Server)
            {
                int item = Item.NewItem(entity.position, entity.Size, itemType, itemStack, true);
                Main.itemLockoutTime[item] = 54000;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        NetMessage.SendData(MessageID.InstancedItem, i, -1, null, item);
                    }
                }

                Main.item[item].active = false;
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Item.NewItem(entity.position, entity.Size, itemType, itemStack);
            }
        }
    }
}
