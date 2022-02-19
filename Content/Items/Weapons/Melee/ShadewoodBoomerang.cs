using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Content.Projectiles.KPlayer.Melee;
using KawaggyMod.Core.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace KawaggyMod.Content.Items.Weapons.Melee
{
    public class ShadewoodBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadewood Boomerang");
            Tooltip.SetDefault("'It feels cursed...'" +
                "\nStacks up to 3" +
                "\nInflicts a weaker Ichor on hit");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.damage = 28;
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
            item.shoot = ModContent.ProjectileType<ShadewoodBoomerangProjectile>();
            item.shootSpeed = 9f;
            item.maxStack = 3;
            item.value = Item.sellPrice(silver: 28);
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenBoomerang);
            recipe.AddIngredient(ItemID.Shadewood, 5);
            recipe.AddIngredient(ModContent.ItemType<BrainOfCthulhuTooth>(), 2);
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
