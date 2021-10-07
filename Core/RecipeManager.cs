using KawaggyMod.Core.Helpers;
using Terraria.ModLoader;
using Terraria.ID;

namespace KawaggyMod.Core
{
    public static class RecipeManager
    {
        public static string CopperBarTier;
        public static string WorldEvilChunk;

        public const string AnyWood = "Wood";
        public const string IronBarTier = "IronBar";

        public static void AddRecipeGroups(Mod mod)
        {
            CopperBarTier = 
            KawaggyHelper.NewRecipeGroup("CopperBar", "Mods.KawaggyMod.Common.CopperBar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });

            WorldEvilChunk = 
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
