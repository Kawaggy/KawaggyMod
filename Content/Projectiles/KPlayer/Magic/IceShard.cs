using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Magic
{
    public class IceShard : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.timeLeft = 180;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Vector2 origin = new Vector2(4, 2);

            int i = (int)projectile.position.X / 16;
            int j = (int)projectile.position.Y / 16;
            Color color = Lighting.GetColor(i, j);
            color *= 0.75f;

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, texture.Frame(), color, projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int x = 0; x < 3; x++)
            {
                Dust.NewDust(projectile.Center + new Vector2(0, 20).RotatedBy(projectile.rotation), 4, 4, DustID.Ice, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0;

            Vector2 positionToCheck = projectile.Center + new Vector2(0, 18).RotatedBy(projectile.rotation);

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, positionToCheck, 2, ref collisionPoint))
                return true;

            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(BuffID.Wet))
            {
                target.DelBuff(target.FindBuffIndex(BuffID.Wet));
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (target.HasBuff(BuffID.Wet))
            {
                target.DelBuff(target.FindBuffIndex(BuffID.Wet));
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }
    }
}
