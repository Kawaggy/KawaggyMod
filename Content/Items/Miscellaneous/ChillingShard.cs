using KawaggyMod.Content.Projectiles.KPlayer.Ranger;
using KawaggyMod.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous
{
    public class ChillingShard : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 7;
            item.knockBack = 2f;
            item.width = 22;
            item.height = 22;
            item.rare = ItemRarityID.Green;
            item.ammo = AmmoID.Bullet;
            item.shoot = ModContent.ProjectileType<ChillingShardProjectile>();
            item.shootSpeed = 4f;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBlock, 5);
            recipe.AddRecipeGroup(RecipeManager.CopperBarTier);
            recipe.AddIngredient(ItemID.Bone);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
