using KawaggyMod.Content.Items.Miscellaneous;
using KawaggyMod.Content.Projectiles.KPlayer.Magic;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Magic
{
    public class IcyBook : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.damage = 32;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.magic = true;
            item.mana = 12;
            item.knockBack = 2f;
            item.shoot = ModContent.ProjectileType<IceShard>();
            item.shootSpeed = 10f;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(ModContent.ItemType<ChillingShard>(), 20);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
