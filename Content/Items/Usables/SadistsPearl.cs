using KawaggyMod.Common.ModWorlds;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Usables
{
    public class SadistsPearl : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sadists Pearl");
            Tooltip.SetDefault("'Looks like someone lost this...'" +
                "\nLost Relic that brings forth doom");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = ItemRarityID.Gray;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = false;
        }

        public override bool CanBurnInLava()
        {
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPCHelper.BossAlive();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            if (!NPCHelper.BossAlive()) //sanity check
            {
                SadisticModeWorld.sadisticMode = !SadisticModeWorld.sadisticMode;
                Main.expertMode = true;

                Main.PlaySound(type: SoundID.Roar, Style: 0);

                string key = SadisticModeWorld.sadisticMode ? "Mods.KawaggyMod.Common.SadisticMode.Enabled" : "Mods.KawaggyMod.Common.SadisticMode.Disabled";

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(newText: Language.GetTextValue(key), color: Color.Red);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.BroadcastChatMessage(text: NetworkText.FromKey(key), color: Color.Red);
                    NetMessage.SendData(msgType: MessageID.WorldData);
                }
            }
            return true;
        }
    }
}
