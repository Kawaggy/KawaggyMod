using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.Players
{
    public partial class KawaggyPlayer : ModPlayer
    {
        public override void ResetEffects()
        {
            ResetRanger();
            ResetSummoner();
        }

        public override void Initialize()
        {
            ResetRanger();
            ResetSummoner();
        }

        public override void UpdateDead()
        {
            ResetRanger();
            ResetSummoner();
        }

        public override void PostUpdate()
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Bouldy>()] <= 0)
            {
                Projectile.NewProjectile(player.position, player.velocity, ModContent.ProjectileType<Bouldy>(), 20, 1f, player.whoAmI);
            }
        }
    }
}
