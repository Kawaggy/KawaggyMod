using KawaggyMod.Core.Helpers;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Content.Items.Miscellaneous.Ammo;

namespace KawaggyMod.Core
{
    public static class RecipeManager
    {
        public static string AnyCopperBarTier;
        public static string AnyWorldEvilChunk;
        public static string AnyWormTooth;

        public const string AnyWood = "Wood";
        public const string IronBarTier = "IronBar";

        public static void AddRecipeGroups(Mod mod)
        {
            AnyCopperBarTier = 
            KawaggyHelper.NewRecipeGroup("CopperBar", "Mods.KawaggyMod.Common.CopperBar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });

            AnyWorldEvilChunk = 
            KawaggyHelper.NewRecipeGroup("WorldEvilChunk", "Mods.KawaggyMod.Common.WorldEvilChunk", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });

            AnyWormTooth =
            KawaggyHelper.NewRecipeGroup("WormTooth", "Mods.KawaggyMod.Common.WormTooth", new int[]
            {
                ItemID.WormTooth,
                ModContent.ItemType<EaterOfWorldsTooth>()
            });
        }

        public static void AddRecipes(Mod mod)
        {

        }

        public static void EditRecipes(Mod mod)
        {
            RecipeFinder finder = new RecipeFinder();
            finder.AddIngredient(ItemID.WormTooth);

            foreach(Recipe recipe in finder.SearchRecipes())
            {
                RecipeEditor e = new RecipeEditor(recipe);
                e.AcceptRecipeGroup(AnyWormTooth);
            }

            finder = new RecipeFinder();
            finder.AddIngredient(ItemID.CursedFlame);
            finder.SetResult(ItemID.CursedDart, 100);

            RecipeEditor editor = new RecipeEditor(finder.FindExactRecipe());
            editor.AddIngredient(ModContent.ItemType<EaterOfWorldsToothDart>(), 100);

            finder = new RecipeFinder();
            finder.AddIngredient(ItemID.Ichor);
            finder.SetResult(ItemID.IchorDart, 100);

            editor = new RecipeEditor(finder.FindExactRecipe());
            editor.AddIngredient(ModContent.ItemType<BrainOfCthulhuToothDart>(), 100);
        }
    }
}
