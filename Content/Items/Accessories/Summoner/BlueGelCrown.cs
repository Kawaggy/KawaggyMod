using KawaggyMod.Common.Players;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Summoner
{
    public class BlueGelCrown : KItem
    {
        public override bool Autoload(ref string name)
        {
            KawaggyPlayerEvents.ModifyHitNPCWithProjEvent += SpawnMiniBlueSlime;
            return base.Autoload(ref name);
        }

        public override string Texture => Assets.Items.Accessories + "Summoner/BlueGelCrown";

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.accessory = true;
            item.value = Item.sellPrice(silver: 27);
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Kawaggy().blueGelCrown = true;
        }

        const float chance = 0.20f;
        private void SpawnMiniBlueSlime(Player player, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
            {
                if (player.Kawaggy().blueGelCrown)
                {
                    if (Main.rand.NextFloat() < chance)
                    {
                        Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<MiniBlueSlime>(), proj.damage.RandomDamage(), knockback, proj.owner, 5.ToSeconds());
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 120);
            recipe.AddRecipeGroup("KawaggyMod:AnyCrown");
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
