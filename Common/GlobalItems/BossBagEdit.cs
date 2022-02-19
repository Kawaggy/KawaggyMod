using KawaggyMod.Content.Items.Miscellaneous.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.GlobalItems
{
    public class BossBagEdit : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                switch (arg)
                {
                    case ItemID.EaterOfWorldsBossBag:
                        player.QuickSpawnItem(ModContent.ItemType<EaterOfWorldsTooth>(), Main.rand.Next(12, 49));
                        break;

                    case ItemID.BrainOfCthulhuBossBag:
                        player.QuickSpawnItem(ModContent.ItemType<BrainOfCthulhuTooth>(), Main.rand.Next(12, 49));
                        break;
                }
            }
        }
    }
}
