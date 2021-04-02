using KawaggyMod.Core.Helpers;
using KawaggyMod.Core.IDs;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.Worlds
{
    public class KawaggyWorld : ModWorld
    {
        public override void PostWorldGen()
        {
            WorldHelper.AddItemToChest(ChestID.LockedShadow, (int)50, ItemID.Obsidian, 5, 20);
        }
    }
}
