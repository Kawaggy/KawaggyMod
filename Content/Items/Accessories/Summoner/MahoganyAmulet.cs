using KawaggyMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Summoner
{
    public class MahoganyAmulet : ModItem
    {
        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 30;
            item.height = 30;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.minionDamageMult *= 0.90f;
            player.statLifeMax2 -= (int)(player.statLifeMax2 * 0.15f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahogany, 15);
            recipe.AddIngredient(ItemID.Rope, 2);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddRecipeGroup(RecipeManager.WorldEvilChunk, 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
