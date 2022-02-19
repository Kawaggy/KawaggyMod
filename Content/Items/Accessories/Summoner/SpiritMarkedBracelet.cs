using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Summoner
{
    public class SpiritMarkedBracelet : ModItem
    {
        public override bool Autoload(ref string name)
        {
            PlayerEvents.OnHitNPCWithProjEvent += SpiritMarkedBraceletEffect;
            return base.Autoload(ref name);
        }

        private void SpiritMarkedBraceletEffect(Player player, Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            int type = ModContent.ProjectileType<SpiritMarkedBraceletProjectile>();
            if (player.Summons().spiritMarkedBracelet)
            {
                if (player.whoAmI == projectile.owner)
                {
                    if (projectile.IsSummon() && projectile.type != type)
                    {
                        if (Main.rand.NextFloat() < 0.25f && ProjectileHelper.Count(type) < player.Summons().maxMiniMinions)
                        {
                            Projectile.NewProjectile(player.position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                        }
                    }
                }
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Marked Bracelet");
            Tooltip.SetDefault("'It's trying to talk to me...'" +
                "\nMakes your summons spawn spirits to help fight you");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 30;
            item.height = 32;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 55);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Summons().spiritMarkedBracelet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RichMahogany, 20);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddRecipeGroup(RecipeManager.AnyWorldEvilChunk, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
