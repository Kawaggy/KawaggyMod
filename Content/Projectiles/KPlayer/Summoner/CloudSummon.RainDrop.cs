using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class RainDrop : ModProjectile
    {
        public const byte water = 0;
        public const byte ice = 1;
        public const byte lava = 2;

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
            projectile.penetrate = 3;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.timeLeft = 180;
            projectile.tileCollide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(rainType);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            rainType = reader.ReadByte();
        }

        public byte rainType = 255;

        public static byte RandomType() => (byte)Main.rand.Next(0, 3);

        public override void AI()
        {
            projectile.SmoothRotate(projectile.velocity.ToRotation() - MathHelper.PiOver2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int[] times = new int[] { 60, 90, 120, 150, 180 };

            switch (rainType)
            {
                case water:
                    if (!target.HasBuff(BuffID.Frostburn))
                    {
                        target.AddBuff(BuffID.Wet, Main.rand.Next(times));
                    }
                    break;

                case ice:
                    target.AddBuff(BuffID.Frostburn, Main.rand.Next(times));
                    break;

                case lava:
                    target.AddBuff(BuffID.OnFire, Main.rand.Next(times));
                    break;
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            switch (rainType)
            {
                case water:
                    target.AddBuff(BuffID.Wet, 120);
                    break;

                case ice:
                    target.AddBuff(BuffID.Frostburn, 120);
                    break;

                case lava:
                    target.AddBuff(BuffID.OnFire, 120);
                    break;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color;

            switch (rainType)
            {
                case water:
                    color = Color.DeepSkyBlue;
                    break;

                case ice:
                    color = Color.LightBlue;
                    break;

                case lava:
                    color = Color.OrangeRed;
                    break;

                default:
                    color = Color.White;
                    break;
            }

            color = Lighting.GetColor((int)projectile.position.X / 16, (int)projectile.position.Y / 16, color);

            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, ModContent.GetTexture(Texture).Frame(), color, projectile.rotation, ModContent.GetTexture(Texture).Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
