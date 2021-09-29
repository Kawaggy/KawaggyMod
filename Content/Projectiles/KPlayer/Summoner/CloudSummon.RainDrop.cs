using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class RainDrop : ModProjectile
    {
        public override string Texture => "KawaggyMod/Assets/Extras/WaterDrop";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.timeLeft = 180;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.SmoothRotate(projectile.velocity.ToRotation() - MathHelper.PiOver2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int[] times = new int[] { 60, 90, 120, 150, 180 };

            if (!target.HasBuff(BuffID.Frostburn))
            {
                target.AddBuff(BuffID.Wet, Main.rand.Next(times));
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (!target.HasBuff(BuffID.Frostburn))
            {
                target.AddBuff(BuffID.Wet, Main.rand.Next(120));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Color.DeepSkyBlue;
            color = Lighting.GetColor((int)projectile.position.X / 16, (int)projectile.position.Y / 16, color);
            float multiplyBy = ((float)projectile.penetrate / (float)projectile.maxPenetrate);
            color *= multiplyBy;
            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, ModContent.GetTexture(Texture).Frame(), color, projectile.rotation, ModContent.GetTexture(Texture).Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
