using KawaggyMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class CloudSummon : ModProjectile
    {
        public int faceFrame;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.minionSlots = 1f;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D face = ModContent.GetTexture("KawaggyMod/Content/Projectiles/KPlayer/Summoner/CloudSummon_Faces");
            faceFrame = (int)MathHelper.Clamp(faceFrame, 0, CustomizationManager.cloudSummon.FrameCount - 1);
            Rectangle frame = face.Frame(10, 1, faceFrame, 0);
            
            if (faceFrame > 10)
            {
                face = CustomizationManager.cloudSummon.cache[faceFrame - 11].texture;
                frame = CustomizationManager.cloudSummon.cache[faceFrame - 11].frame;
            }

            if (faceFrame == 10 && ModCompatibilityManager.junkoAndFriends == null)
            {
                faceFrame = 9;
            }
            else if (faceFrame == 10 && ModCompatibilityManager.junkoAndFriends != null)
            {
                face = ModContent.GetTexture("KawaggyMod/Content/Projectiles/KPlayer/Summoner/CloudSummon_FumoFace");
                frame = face.Frame();
            }

            Vector2 origin = projectile.Size / 2f;

            spriteBatch.Draw(face, projectile.Center - Main.screenPosition, frame, lightColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(faceFrame);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            faceFrame = reader.ReadInt32();
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > Main.projFrames[projectile.type] - 1)
                    projectile.frame = 0;
            }
        }

        bool FirstTick = true;
        public override void PostAI()
        {
            if (FirstTick)
            {
                projectile.netUpdate = true;
                FirstTick = false;
            }
        }
    }
}
