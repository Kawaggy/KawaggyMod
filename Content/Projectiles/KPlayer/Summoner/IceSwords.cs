using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class IceSwords : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5; //drawing manually so it shouldn't matter!
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.minionSlots = 1f;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);

            projectile.frame = (int)MathHelper.Clamp(projectile.frame, 0, CustomizationManager.iceSwords.FrameCount - 1);
            Rectangle theFrame = new Rectangle(projectile.frame * 18, 0, 18, 44);

            if (projectile.frame > 4)
            {
                texture = CustomizationManager.iceSwords.cache[projectile.frame - 5].texture;
                theFrame = CustomizationManager.iceSwords.cache[projectile.frame - 5].frame;
            }

            Vector2 origin = new Vector2(9, 6);

            int i = (int)projectile.position.X / 16;
            int j = (int)projectile.position.Y / 16;
            Color color = Lighting.GetColor(i, j);
            color *= 0.75f;
            
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, theFrame, color, projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.frame);
            writer.Write(projectile.localAI[0]);
            writer.Write(projectile.localAI[1]);

            writer.Write(random.X);
            writer.Write(random.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.frame = reader.ReadInt32();
            projectile.localAI[0] = reader.ReadSingle();
            projectile.localAI[1] = reader.ReadSingle();

            float X = reader.ReadSingle();
            float Y = reader.ReadSingle();
            random = new Vector2(X, Y);
        }

        //ai0 = state
        //ai1 = counter
        //localai0 = flag
        //localai1 = random time offset

        private Vector2 random;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<IceSwordsBuff>());

            if (player.HasBuff(ModContent.BuffType<IceSwordsBuff>()))
                projectile.timeLeft = 2;

            projectile.TeleportIfTooFarFrom(player.Center, 2200f);

            projectile.hide = false;

            (int myCount, int _) = projectile.CountSameAsSelf(true);
            float XOffset = ((myCount - 1) % 2 == 0 ? 1 : -1) * 20 * myCount * player.direction * -1;
            Vector2 idlePosition = player.Center + new Vector2(XOffset, -100);

            switch (projectile.ai[0])
            {
                case 0: //idle state
                    projectile.ai[1]++;

                    //idle (falling down)
                    if (projectile.ai[1] > 60 * 10 && projectile.DistanceSQ(idlePosition) < 800f * 800f)
                    {
                        if (projectile.ai[1] == (60 * 10) + 1)
                            projectile.velocity.Y -= 4f;

                        projectile.hide = true; //to draw behind tiles properly!
                        projectile.velocity *= 0.98f; //to slow down all movement!

                        Vector2 vectorToCheck = projectile.Center + new Vector2(0, 20).RotatedBy(projectile.rotation);
                        int i = (int)vectorToCheck.X / 16;
                        int j = (int)vectorToCheck.Y / 16;

                        if (!WorldGen.SolidOrSlopedTile(i, j) && Main.tile[i, j].type != TileID.Platforms)
                        {
                            projectile.velocity.Y += ProjectileHelper.gravity / 2f; //makes it look better!

                            if (projectile.velocity.Y > 10f)
                                projectile.velocity.Y = 10f;

                            float forRotation = projectile.oldPosition.X - projectile.position.X;
                            forRotation *= 1.5f;
                            projectile.SmoothRotate(MathHelper.Clamp(forRotation, -2.75f, 2.75f) * 0.2f, 0.05f);
                        }
                        //idle (in ground)
                        else
                        {
                            if (projectile.localAI[0] == 0)
                            {
                                projectile.netUpdate = true;
                                projectile.localAI[0] = 1;
                                Main.PlaySound(SoundID.Tink, projectile.position);
                                for (int x = 0; x < 3; x++)
                                {
                                    Dust.NewDust(projectile.Center + new Vector2(0, 20).RotatedBy(projectile.rotation), 4, 4, DustID.Ice, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
                                }
                                for (int x = 0; x < 3; x++)
                                {
                                    WorldGen.KillTile(i, j, true, true);
                                }
                            }

                            projectile.velocity.X *= 0.75f;
                            projectile.velocity.Y *= 0f;
                        }
                    }
                    //flying around idle state
                    else
                    {
                        projectile.localAI[0] = 0;
                        float forRotation = projectile.position.X - projectile.oldPosition.X;
                        projectile.SmoothRotate(MathHelper.Clamp(forRotation, -2.75f, 2.75f) * 0.2f);
                        Move(idlePosition);
                    }

                    //check for enemy (or firefly or lightning bug)
                    bool near = false;
                    float distance = float.MaxValue;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && ((npc.type != NPCID.TargetDummy && !npc.friendly && npc.lifeMax > 5 && npc.CanBeChasedBy()) || npc.type == NPCID.Firefly || npc.type == NPCID.LightningBug))
                        {
                            float npcDistance = projectile.Distance(npc.Center);
                            if (npcDistance < 1800f && player.DistanceSQ(npc.Center) < 2000f * 2000f)
                            {
                                if (!near)
                                    near = true;
                                if (npcDistance < distance)
                                    distance = npcDistance;
                            }
                        }
                    }

                    //enemy near
                    if (near)
                        projectile.ai[1] = 0;

                    //player moved
                    if ((player.oldPosition - player.position).Length() > 3)
                        projectile.ai[1] = 0;

                    //enemy too near
                    if (distance < 950f)
                        ResetMe(1);
                    break;

                case 1:
                    //target search
                    distance = float.MaxValue;
                    int target = -1;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && ((npc.type != NPCID.TargetDummy && !npc.friendly && npc.lifeMax > 5 && npc.CanBeChasedBy()) || npc.type == NPCID.Firefly || npc.type == NPCID.LightningBug))
                        {
                            float npcDistance = projectile.Distance(npc.Center);
                            if (npcDistance < 1200f)
                            {
                                if (npcDistance < 950f && distance > npcDistance)
                                    target = i;

                                if (distance > npcDistance)
                                    distance = npcDistance;
                            }

                            if (player.DistanceSQ(npc.Center) > 2000f * 2000f)
                                ResetMe(0);
                        }
                    }

                    //player has target selected
                    if (player.HasMinionAttackTargetNPC)
                    {
                        NPC npc = Main.npc[player.MinionAttackTargetNPC];
                        float between = Vector2.Distance(npc.Center, projectile.Center);
                        if (between < 2000f)
                            target = npc.whoAmI;
                    }

                    if (distance > 1200f)
                    {
                        ResetMe(0);
                    }
                    else if (distance > 950f) //"idle"
                    {
                        if (projectile.localAI[0] != 1)
                        {
                            projectile.localAI[0] = 1;
                            projectile.ai[1] = 0;
                            projectile.netUpdate = true;
                        }
                        if (++projectile.ai[1] > 120)
                        {
                            ResetMe(0);
                        }

                        float forRotation = projectile.position.X - projectile.oldPosition.X;
                        projectile.SmoothRotate(MathHelper.Clamp(forRotation, -2.75f, 2.75f) * 0.2f);
                        Move(idlePosition);
                    }

                    if (target != -1)
                    {
                        if (projectile.localAI[0] != 2) //random offset and random time
                        {
                            projectile.localAI[0] = 2;
                            random = new Vector2(0, -240).RotatedBy(MathHelper.TwoPi);
                            projectile.localAI[1] = Main.rand.Next(-30, 61);
                            projectile.netUpdate = true;
                        }

                        if (++projectile.ai[1] > 120 + projectile.localAI[1]) //charge!
                        {
                            if (projectile.ai[1] > 150 + projectile.localAI[1]) //actual charge
                            {
                                projectile.localAI[1] = Main.rand.Next(-30, 61);
                                projectile.ai[1] = -15;

                                Vector2 chargeVelocity = Main.npc[target].Center - projectile.Center;
                                chargeVelocity.Normalize();
                                chargeVelocity *= 16f;
                                projectile.velocity = chargeVelocity;
                                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                                random = new Vector2(0, -240).RotatedBy(MathHelper.TwoPi);
                                projectile.netUpdate = true;
                            }
                            //move out a little
                            projectile.SmoothRotate((Main.npc[target].Center - projectile.Center).ToRotation() - MathHelper.PiOver2);
                            Move(Main.npc[target].Center + random + new Vector2(0, projectile.ai[1]).RotatedBy(projectile.rotation));
                        }
                        else if (projectile.ai[1] > 0) //idle around the npc
                        {
                            projectile.SmoothRotate((Main.npc[target].Center - projectile.Center).ToRotation() - MathHelper.PiOver2);
                            Move(Main.npc[target].Center + random);
                        }
                    }
                    break;
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

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Shatter, projectile.position);

            for (int x = 0; x < 3; x++)
            {
                Dust.NewDust(projectile.Center + new Vector2(0, 20).RotatedBy(projectile.rotation), 4, 4, DustID.Ice, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0;

            Vector2 positionToCheck = projectile.Center + new Vector2(0, 40).RotatedBy(projectile.rotation);

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, positionToCheck, 2, ref collisionPoint))
                return true;

            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool MinionContactDamage()
        {
            return true;
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

        internal void Move(Vector2 position)
        {
            if (Main.rand.Next(100) == 0)
                Dust.NewDust(projectile.Center + new Vector2(0, 20).RotatedBy(projectile.rotation), 4, 4, DustID.Ice, Main.rand.Next(-1, 2) / 2, 0.5f);
            if (projectile.DistanceSQ(position) > 10f * 10f)
            {
                Vector2 velocity = position - projectile.Center;
                velocity.Normalize();
                velocity *= 6f;
                projectile.velocity = (projectile.velocity * (40 - 1) + velocity) / 40;
            }
        }

        internal void ResetMe(int nextState)
        {
            projectile.netUpdate = true;
            projectile.ai[0] = nextState;
            projectile.ai[1] = 0;
            projectile.localAI[0] = 0;
        }
    }
}
