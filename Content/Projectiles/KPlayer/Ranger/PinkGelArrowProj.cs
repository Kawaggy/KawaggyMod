using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Content.Projectiles.KPlayer.Ranger
{
    public class PinkGelArrowProj : KProjectile
    {
        public override string Texture => Assets.Projectiles.Player + "Ranger/PinkGelArrowProj";

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = 20.ToSeconds();
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCKilled, projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, new Color(255, 80, 170));
            }
        }

        public int Bounces { get => (int)projectile.ai[0]; set => projectile.ai[0] = value; }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.netUpdate = true;
            if (Bounces > 0)
            {
                Main.PlaySound(SoundID.Item10, projectile.position);
                Bounces--;
                projectile.velocity.Y = -oldVelocity.Y;
                int i = (int)((projectile.position.X + (projectile.width / 2)) / 16);
                int j = (int)((projectile.position.Y + (projectile.height / 2)) / 16);
                i += projectile.direction;

                if (WorldGen.SolidTile(i, j))
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                projectile.velocity *= 0.95f;

                Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, new Color(255, 80, 170));
                return false;
            }
            return Bounces <= 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Bounces <= 0)
            {
                projectile.netUpdate = true;
                projectile.Kill();
            }

            if (Bounces > 0)
            {
                projectile.netUpdate = true;
                Bounces--;
                projectile.velocity = -projectile.oldVelocity;
                projectile.velocity *= 0.98f;

                Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, new Color(255, 80, 170));
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 50)
                projectile.alpha -= 15;
            if (projectile.alpha < 50)
                projectile.alpha = 50;

            projectile.velocity.Y += 0.2f;

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            projectile.CapSpeed(16f);
        }
    }
}
