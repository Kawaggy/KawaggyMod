using KawaggyMod.Content.Buffs.Debuffs;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Ranger
{
    // Name really couldn't be any longer, hm?
    public class BrainOfCthulhuToothDartProjectile : ModProjectile
    {
        public override string Texture => "KawaggyMod/Content/Items/Miscellaneous/Ammo/BrainOfCthulhuToothDart";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain of Cthulhu Tooth Dart");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
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
            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition,ModContent.GetTexture(Texture).Frame(), lightColor * (projectile.alpha / 255f), projectile.rotation, ModContent.GetTexture(Texture).Size() / 2f, projectile.scale, projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }

        public override void AI()
        {
            if (projectile.alpha < 255)
                projectile.alpha += 16;

            if (projectile.alpha > 255)
                projectile.alpha = 255;

            if (projectile.velocity.X > 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi / 8f;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi / 16f;
            }
            if (++projectile.ai[0] > 30)
            {
                projectile.velocity.X *= 0.98f;
                projectile.velocity.Y += ProjectileHelper.gravity;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WeakerIchor>(), (int)(60 * 5f));
            AddDust();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WeakerIchor>(), (int)(60 * 5f));
            AddDust();
        }

        void AddDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.IchorTorch, -projectile.velocity.X / 2f, -projectile.velocity.Y / 2f);
            }
        }

        public override void Kill(int timeLeft)
        {
            AddDust();
        }
    }
}
