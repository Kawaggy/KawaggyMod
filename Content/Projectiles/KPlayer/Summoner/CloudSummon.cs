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

            DisplayName.SetDefault("Cloud");
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

            wasUsedForJumping = false;
            canJump = false;
            canAttack = false;
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
            writer.Write(wasUsedForJumping);
            writer.Write(canJump);
            writer.Write(canAttack);
            writer.Write(projectile.localAI[0]);
            writer.Write(projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            faceFrame = reader.ReadInt32();
            wasUsedForJumping = reader.ReadBoolean();
            canJump = reader.ReadBoolean();
            canAttack = reader.ReadBoolean();
            projectile.localAI[0] = reader.ReadSingle();
            projectile.localAI[1] = reader.ReadSingle();
        }

        public override void Kill(int timeLeft)
        {
            SpawnDust();
        }

        internal void SpawnDust()
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Cloud, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), Scale: Main.rand.NextFloat(1, 1.5f));
            }
        }

        public bool wasUsedForJumping;
        public bool canJump;
        public bool canAttack;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<CloudSummonBuff>());

            if (player.HasBuff(ModContent.BuffType<CloudSummonBuff>()))
                projectile.timeLeft = 2;

            projectile.SimpleAnimation(animationSpeed: 5);

            projectile.TeleportIfTooFarFrom(player.Center, 1800f, SpawnDust);

            projectile.velocity *= 0.96f;
            (int myCount, int fullCount) = projectile.CountSameAsSelf(true);
            Vector2 idlePosition = player.Center + new Vector2(0, -120).RotatedBy((MathHelper.TwoPi / fullCount) * (myCount));

            NPCData closestNPC = projectile.FindClosest<NPC>(delegate (NPC npc) { return !npc.friendly && npc.CanBeChasedBy(); });

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];

                if (npc.active && projectile.Distance(npc.Center) < 800f)
                {
                    closestNPC.npc = npc;
                    closestNPC.distance = projectile.Distance(npc.Center);
                    closestNPC.hasLineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, closestNPC.npc.position, closestNPC.npc.width, closestNPC.npc.height);
                }
            }

            if (player.AllMobilityDepleted() && !player.mount.Active && player.jump <= 0)
            {
                if (projectile.localAI[1] != 1)
                {
                    canJump = true;
                    wasUsedForJumping = false;
                }
                projectile.localAI[1] = 1; //aka can jump
            }
            else
            {
                if (projectile.localAI[1] != 0)
                {
                    canJump = false;
                }
                projectile.localAI[1] = 0;
            }

            if (closestNPC.distance <= (!player.HasMinionAttackTargetNPC ? 500f : 800f))
            {
                if (projectile.localAI[0] != 1)
                {
                    canAttack = true;
                }
                projectile.localAI[0] = 1; //aka can attack
            }
            else
            {
                if (projectile.localAI[0] != 0)
                {
                    canAttack = false;
                }
                projectile.localAI[0] = 0;
            }

            bool otherActive = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.owner == projectile.owner && other.minion && other.minionSlots > 0)
                {
                    if (other.type != projectile.type)
                    {
                        otherActive = true;
                        break;
                    }
                }
            }

            if (canJump)
            {
                if (player.Summons().currentCloudJump == -1 && !wasUsedForJumping)
                {
                    if (canAttack)
                    {
                        if (myCount != 1 || otherActive)
                        {
                            player.Summons().currentCloudJump = projectile.whoAmI;
                            SpawnDust();
                            projectile.netUpdate = true;
                        }
                    }
                    else if (!canAttack)
                    {
                        player.Summons().currentCloudJump = projectile.whoAmI;
                        SpawnDust();
                        projectile.netUpdate = true;
                    }
                }

                if (player.Summons().currentCloudJump == projectile.whoAmI)
                {
                    if (player.Summons().jumpAgainCloudCounter <= 0)
                    {
                        if (player.controlJump && player.Kawaggy().oldJump)
                        {
                            player.velocity.Y = 0;
                            player.velocity.Y = 0 - Player.jumpSpeed * 2f * player.gravDir;
                            player.Summons().jumpAgainCloudCounter = 30;
                            player.Summons().currentCloudJump = -1;
                            wasUsedForJumping = true;
                            projectile.velocity.Y += 8f * player.gravDir;
                            player.fallStart = (int)player.position.Y / 16;
                            projectile.netUpdate = true;
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2 velocity = new Vector2(0, -4f * player.gravDir).RotatedByRandom(MathHelper.Pi / 16);
                                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Cloud, velocity.X, velocity.Y, Scale: Main.rand.NextFloat(1.5f, 2f));
                            }
                        }
                    }
                    Vector2 position = (player.Bottom + new Vector2(0, 16));
                    projectile.rotation = 0f;

                    if (player.gravDir == -1)
                    {
                        projectile.rotation = MathHelper.Pi;
                        position = (player.Top + new Vector2(0, -16f));
                    }

                    projectile.Center = position;
                    return;
                }
            }

            if (((player.velocity.Y == 0 || player.sliding) && player.releaseJump) || (player.autoJump && player.justJumped))
                wasUsedForJumping = false;

            if (canAttack)
            {
                Vector2 vectorToNPC = closestNPC.npc.Center + new Vector2(0, -120).RotatedBy(((MathHelper.TwoPi / fullCount) * myCount) + player.Kawaggy().rotation);
                Vector2 directionToNPC = closestNPC.npc.Center - projectile.Center;
                directionToNPC.Normalize();

                Move(vectorToNPC);
                projectile.SmoothRotate(directionToNPC.ToRotation() - MathHelper.PiOver2);
                if (++projectile.ai[0] >= 20)
                {
                    projectile.ai[0] = 0;
                    Vector2 randomOffset = new Vector2(0, 1).RotatedByRandom(MathHelper.TwoPi);
                    Projectile rainDrop = Projectile.NewProjectileDirect(projectile.Center, (directionToNPC * 6f) + randomOffset, ModContent.ProjectileType<RainDrop>(), projectile.damage, 0f, projectile.owner, ProjectileHelper.gravity, 0.98f);
                    projectile.netUpdate = true;
                }
                return;
            }    

            if (!canAttack || wasUsedForJumping)
            {
                Move(idlePosition);
                projectile.SmoothRotate(projectile.velocity.X * 0.2f);
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
