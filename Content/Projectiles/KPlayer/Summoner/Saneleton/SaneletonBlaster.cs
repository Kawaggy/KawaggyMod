using KawaggyMod.Core.ModTypes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner.Saneleton
{
    public class SaneletonBlaster : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.height = 26;
            projectile.width = 20;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center + new Vector2(-16 * 4, -16 * 4);

            if (++projectile.ai[0] > 240)
            {
                Projectile ray = Projectile.NewProjectileDirect(projectile.Center, projectile.DirectionTo(Main.player[projectile.owner].Center), ModContent.ProjectileType<SaneletonDeathray>(), projectile.damage, projectile.knockBack, projectile.owner, -MathHelper.TwoPi / 240);
                (ray.modProjectile as ModDeathray).entityOwner = projectile.whoAmI;
                projectile.ai[0] = 0;
            }
        }
    }
}
