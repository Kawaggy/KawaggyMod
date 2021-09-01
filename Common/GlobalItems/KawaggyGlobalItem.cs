using KawaggyMod.Common.Configs;
using KawaggyMod.Core.Interfaces;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Common.GlobalItems
{
    public class KawaggyGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.modItem is ICustomizable customized)
            {
                if (ModContent.GetInstance<DebugConfig>().ShowExtraSpritesCount)
                {
                    string theText = Language.GetTextValue($"Mods.KawaggyMod.Common.AmountOfExtraSprites", customized.Count);
                    tooltips.Add(new TooltipLine(mod, "AmountOfSprites", theText));
                }
            }
        }
    }
}
