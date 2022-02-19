using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Content.Projectiles.KPlayer.Ranger;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Ammo
{
    public class EaterOfWorldsToothDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eater of Worlds Tooth Dart");
            Tooltip.SetDefault("Inflicts Cursed Flames on hit");
        }

        public override void SetDefaults()
        {
            item.damage = 5;
            item.knockBack = 1f;
            item.ranged = true;
            item.width = 10;
            item.height = 16;
            item.rare = ItemRarityID.Blue;
            item.ammo = AmmoID.Dart;
            item.shoot = ModContent.ProjectileType<EaterOfWorldsToothDartProjectile>();
            item.shootSpeed = 2.5f;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EaterOfWorldsTooth>());
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
