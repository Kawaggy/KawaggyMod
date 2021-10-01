using KawaggyMod.Core.Helpers;
using Terraria.ModLoader;
using Terraria.ID;

namespace KawaggyMod.Core
{
    public static class RecipeManager
    {
        public const string CopperBarTier = "KawaggyMod:CopperBar";
        public const string WorldEvilChunk = "KawaggyMod:WorldEvilChunk";

        public const string AnyWood = "Wood";
        public const string IronBarTier = "IronBar";

        public static void AddRecipeGroups(Mod mod)
        {
            KawaggyHelper.NewRecipeGroup("CopperBar", "Mods.KawaggyMod.Common.CopperBar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });

            KawaggyHelper.NewRecipeGroup("WorldEvilChunk", "Mods.KawaggyMod.Common.WorldEvilChunk", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
        }

        public static void AddRecipes(Mod mod)
        {

        }

        public static void EditRecipes(Mod mod)
        {

        }
    }
}
