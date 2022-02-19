using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Content.Projectiles.KPlayer.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Ammo
{
    public class BrainOfCthulhuToothDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain of Cthulhu Tooth Dart");
            Tooltip.SetDefault("Inflicts a weaker Ichor on hit");
        }

        public override void SetDefaults()
        {
            item.damage = 6;
            item.knockBack = 1.1f;
            item.ranged = true;
            item.width = 10;
            item.height = 16;
            item.rare = ItemRarityID.Blue;
            item.ammo = AmmoID.Dart;
            item.shoot = ModContent.ProjectileType<BrainOfCthulhuToothDartProjectile>();
            item.shootSpeed = 2.5f;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BrainOfCthulhuTooth>());
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
