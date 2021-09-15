using KawaggyMod.Common.ModWorlds;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Helpers
{
    public class CustomRarity
    {
        public class Developer
        {
            public const int Yharex = 87;
        }
    }

    public static class ItemHelper
    {
        public static void CheckRarity(int rare, ref List<TooltipLine> tooltips)
        {
            if (rare <= 12)
                return;

            Color color;

            switch(rare)
            {
                case CustomRarity.Developer.Yharex:
                    color = Color.Lerp(new Color(245, 123, 66), new Color(245, 230, 66), RarityCounterModWorld.fourSecondColorLerp);
                    break;
                default:
                    color = Color.Lerp(Color.Magenta, Color.Black, RarityCounterModWorld.fourSecondColorLerp);
                    break;
            }

            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = color;
                }
            }
        }
    }
}
