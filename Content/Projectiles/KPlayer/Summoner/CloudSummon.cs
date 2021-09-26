using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.DataTypes;
using KawaggyMod.Core.Helpers;
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
            Player player = Main.player[projectile.owner];

            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<CloudSummonBuff>());

            if (player.HasBuff(ModContent.BuffType<CloudSummonBuff>()))
                projectile.timeLeft = 2;

            projectile.TeleportIfTooFarFrom(player.Center, 1800f);

            projectile.velocity *= 0.96f;

            Vector2 idlePosition = player.Center + new Vector2(0, -120).RotatedBy((MathHelper.TwoPi / projectile.CountSameAsSelf(true)) * (projectile.minionPos + 1));
            /*
            bool flying = false;
            bool allJumpsDepleted = !player.jumpAgainBlizzard && !player.jumpAgainCloud && !player.jumpAgainFart && !player.jumpAgainSail && !player.jumpAgainSandstorm && !player.jumpAgainUnicorn;

            if (player.wingsLogic > 0 && player.controlJump && player.wingTime > 0f && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f)
                flying = true;

            if ((player.wingsLogic == 22 || player.wingsLogic == 28 || player.wingsLogic == 30 || player.wingsLogic == 32 || player.wingsLogic == 29 || player.wingsLogic == 33 || player.wingsLogic == 35 || player.wingsLogic == 37) && player.controlJump && player.controlDown && player.wingTime > 0f)
                flying = true;

            if (player.rocketTime != 0)
                flying = true;
            */
            NPCData closestNPC = projectile.FindClosest<NPC>(delegate (NPC npc) { return npc.life > 5 && !npc.friendly && npc.CanBeChasedBy(); }, true);

            if (player.AllMobilityDepleted() && !player.mount.Active && player.jump <= 0 && closestNPC.distance > 500f)
            {
                projectile.ai[0] = 1;
            }
            else
            {
                projectile.ai[0] = 0;
            }

            if (player.velocity.Y == 0)
                projectile.ai[0] = 0;

            if (closestNPC.distance <= 500f)
                projectile.ai[0] = 2;

            switch (projectile.ai[0])
            {
                case 0: //idle
                    projectile.ai[1] = 0;
                    projectile.SmoothRotate(projectile.velocity.X * 0.2f, 0.2f);
                    Move(idlePosition);
                    break;

                case 1: //for jumping!
                    if (projectile.ai[1] == 0)
                    {
                        if (player.Summons().currentCloudJump == -1)
                        {
                            player.Summons().currentCloudJump = projectile.whoAmI;
                        }
                    }
                    
                    if (player.Summons().currentCloudJump == projectile.whoAmI)
                    {
                        if (player.Summons().jumpAgainCloudCounter <= 0)
                        {
                            if (player.controlJump && player.Kawaggy().oldJump)
                            {
                                player.velocity.Y = 0;
                                player.velocity.Y = 0f - Player.jumpSpeed * 2f * player.gravDir;
                                player.Summons().jumpAgainCloudCounter = 30;
                                player.Summons().currentCloudJump = -1;
                                projectile.ai[1] = 2;
                                projectile.velocity.Y += 8f;
                                player.fallStart = (int)player.position.Y / 16;
                            }
                        }
                        Vector2 position = (player.Bottom + new Vector2(0, 16f));
                        projectile.rotation = 0f;

                        if (player.gravDir == -1)
                        {
                            projectile.rotation = MathHelper.Pi;
                            position = (player.Top + new Vector2(0, -16f));
                        }
                        projectile.Center = position;
                    }
                    else
                    {
                        projectile.SmoothRotate(projectile.velocity.X * 0.2f, 0.2f);
                        Move(idlePosition);
                    }
                    break;

                case 2: //npc attacking

                    break;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > Main.projFrames[projectile.type] - 1)
                    projectile.frame = 0;
            }
        }

        internal void Move(Vector2 position)
        {
            if (projectile.DistanceSQ(position) > 10f * 10f)
            {
                Vector2 velocity = position - projectile.Center;
                velocity.Normalize();
                velocity *= 10f;
                projectile.velocity = (projectile.velocity * (40 - 1) + velocity) / 40f;
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
