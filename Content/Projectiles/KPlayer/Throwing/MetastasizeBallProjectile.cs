using KawaggyMod.Content.Buffs.Debuffs;
using KawaggyMod.Content.Projectiles.KPlayer.Ranger;
using KawaggyMod.Core.ModTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Throwing
{
    public class MetastasizeBallProjectile : ModGrenade
    {
        public override string Texture => "KawaggyMod/Content/Items/Weapons/Throwing/MetastasizeBall";

        public override bool Sticky => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Metastasize Ball");
        }

        public override void SetDefaults()
        {
            Defaults(60 * 4, 22);
        }

        public override bool ModifyFireDust(ref int type)
        {
            type = DustID.Ichor;
            return true;
        }

        public override bool ModifyOwnerHurt(Player player)
        {
            player.AddBuff(ModContent.BuffType<WeakerIchor>(), (int)(60 * 2.5f));
            return true;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WeakerIchor>(), (int)(60 * 5f));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WeakerIchor>(), (int)(60 * 5f));
        }

        public override void OnKill()
        {
            if (Main.myPlayer == projectile.owner)
            {
                for (int i = -2; i < 2; i++)
                {
                    Projectile newProjectile = Projectile.NewProjectileDirect(projectile.Center, new Vector2(0, 6).RotatedBy(((MathHelper.TwoPi / 4) * i) + projectile.rotation), ModContent.ProjectileType<BrainOfCthulhuToothDartProjectile>(), projectile.damage / 6, projectile.knockBack / 4f, projectile.owner);
                    newProjectile.ranged = false;
                    newProjectile.thrown = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, ModContent.GetTexture(Texture).Size() / 2f, projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
