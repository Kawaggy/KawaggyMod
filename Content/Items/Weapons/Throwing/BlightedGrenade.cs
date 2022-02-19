using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Content.Projectiles.KPlayer.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Throwing
{
    public class BlightedGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blighted Grenade");
            Tooltip.SetDefault("Bouncy" +
                "\nInflicts Cursed Flames on explosion" +
                "\nOn explosion releases teeth that inflict Cursed Flames");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shootSpeed = 5.5f;
            item.shoot = ModContent.ProjectileType<BlightedGrenadeProjectile>();
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.consumable = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 45;
            item.useTime = 45;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.value = Item.sellPrice(silver: 1, copper: 50);
            item.damage = 50;
            item.knockBack = 8f;
            item.thrown = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BouncyGrenade, 5);
            recipe.AddIngredient(ModContent.ItemType<EaterOfWorldsTooth>());
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
        }
    }
}
