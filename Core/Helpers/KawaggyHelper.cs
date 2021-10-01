using Terraria;
using Terraria.Localization;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static void NewRecipeGroup(string name, string description, int[] itemList)
        {
            RecipeGroup.RegisterGroup(
                "KawaggyMod:" + name,
                new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue(description)}",
                itemList));
        }
    }
}
