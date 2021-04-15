using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Summoner
{
    public class StrangeLookingActiveStoneBlock : KItem
    {
        public override string Texture => Assets.Items.Weapons + "Summoner/StrangeLookingActiveStoneBlock";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 24;
            item.damage = 12;
            item.mana = 10;
            item.knockBack = 3f;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<BouldySummonBuff>();
            item.shoot = ModContent.ProjectileType<Bouldy>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Boulder);
            recipe.AddIngredient(ItemID.ActiveStoneBlock, 6);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
