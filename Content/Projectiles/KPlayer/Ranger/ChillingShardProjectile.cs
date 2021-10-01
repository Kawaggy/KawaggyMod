using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Ranger
{
    public class ChillingShardProjectile : ModProjectile
    {
        public override string Texture => "KawaggyMod/Content/Items/Miscellaneous/ChillingShard";

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 2;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 45;
            projectile.timeLeft = 600;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, ModContent.GetTexture(Texture).Frame(), lightColor, projectile.rotation, ModContent.GetTexture(Texture).Size() / 2f, projectile.scale, projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }

        public override void AI()
        {
            if (projectile.velocity.X > 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            if (++projectile.ai[0] > 30)
            {
                projectile.velocity.X *= 0.98f;
                projectile.velocity.Y += ProjectileHelper.gravity;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(BuffID.Wet))
            {
                target.DelBuff(target.FindBuffIndex(BuffID.Wet));
                target.AddBuff(BuffID.Frostburn, 90);
            }
            else if (!target.HasBuff(BuffID.Frostburn))
            {
                target.AddBuff(BuffID.Wet, 90);
            }
            AddDust();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (target.HasBuff(BuffID.Wet))
            {
                target.DelBuff(target.FindBuffIndex(BuffID.Wet));
                target.AddBuff(BuffID.Frostburn, 90);
            }
            else if (!target.HasBuff(BuffID.Frostburn))
            {
                target.AddBuff(BuffID.Wet, 90);
            }
            AddDust();
        }

        void AddDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ice, -projectile.velocity.X / 2f, -projectile.velocity.Y / 2f);
            }
        }

        public override void Kill(int timeLeft)
        {
            AddDust();
        }
    }
}
