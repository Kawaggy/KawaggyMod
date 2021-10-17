using KawaggyMod.Core.ModTypes;
using Terraria.ID;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner.Saneleton
{
    public class SaneletonDeathray : ModDeathray
    {
        public override EntityContext Context => EntityContext.Projectile;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.timeLeft = 240;
        }
    }
}
