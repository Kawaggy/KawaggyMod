using KawaggyMod.Content.Projectiles.KPlayer.Melee;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using KawaggyMod.Content.Items.Miscellaneous.Materials;
using Terraria.Utilities;
using KawaggyMod.Core.Helpers;

namespace KawaggyMod.Content.Items.Weapons.Melee
{
    public class EbonwoodBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebonwood Boomerang");
            Tooltip.SetDefault("'It feels cursed...'" +
                "\nStacks up to 3" +
                "\nInflicts Cursed Flames on hit");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.damage = 24;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.useAnimation = 24;
            item.useTime = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.rare = ItemRarityID.Blue;
            item.shoot = ModContent.ProjectileType<EbonwoodBoomerangProjectile>();
            item.shootSpeed = 9f;
            item.maxStack = 3;
            item.value = Item.sellPrice(silver: 20);
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenBoomerang);
            recipe.AddIngredient(ItemID.Ebonwood, 5);
            recipe.AddIngredient(ModContent.ItemType<EaterOfWorldsTooth>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            return ProjectileHelper.Count(item.shoot, player.whoAmI) < item.stack;
        }
    }
}
