using KawaggyMod.Content.Items.Accessories.Ranger;
using KawaggyMod.Content.Items.Accessories.Summoner;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.GlobalItems
{
    public partial class KGlobalItem : GlobalItem
    {
        public delegate void OpenVanillaBagDelegate(string context, Player player, int arg);
        public static event OpenVanillaBagDelegate OpenVanillaBagEvent;
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            OpenVanillaBagEvent?.Invoke(context, player, arg);

            if (context == "bossBag")
            {
                if (arg == ItemID.KingSlimeBossBag)
                {
                    if (Main.rand.Next(20) == 0)
                    {
                        List<int> choices = new List<int>
                        {
                            ModContent.ItemType<BlueGelCrown>(),
                            ModContent.ItemType<PinkGelCrown>(),
                            ModContent.ItemType<BlueGelArrow>(),
                            ModContent.ItemType<PinkGelArrow>()
                        };

                        player.QuickSpawnItem(choices[Main.rand.Next(choices.Count)]);
                    }
                }
            }
        }
    }
}
