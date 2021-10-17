using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
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
            PlayerEvents.OnHitPlayerEvent += SpiritMarkedBraceletEffect;
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
                        if (Main.rand.NextFloat() < 0.25f && ProjectileHelper.Count(type) < 5)
                        {
                            Projectile.NewProjectile(player.position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                        }
                    }
                }
            }
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 30;
            item.height = 32;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Summons().spiritMarkedBracelet = true;
        }
    }
}
