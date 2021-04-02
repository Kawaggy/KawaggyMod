using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Helpers
{
    public static class ItemHelper
    {
        //will become obsolete in 1.14
        public static void NewRarity(List<TooltipLine> tooltips, int rare, bool sadistic) //redo for new things
        {
            if (rare <= 11 && rare >= -1)
                return;

            Color color;
            switch (rare)
            {
                default:
                    color = Color.White;
                    break;
            }
            if (sadistic)
            {
                color = new Color(120, 0, 0);
            }

            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = color;
                }
            }
        }
        public static void NewRarity(List<TooltipLine> tooltips, bool sadistic) => NewRarity(tooltips, -2, sadistic);
        public static void NewRarity(List<TooltipLine> tooltips, int rare) => NewRarity(tooltips, rare, false);
        //till here ^
        public static void NewRarity(List<TooltipLine> tooltips, Color color)
        {
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
