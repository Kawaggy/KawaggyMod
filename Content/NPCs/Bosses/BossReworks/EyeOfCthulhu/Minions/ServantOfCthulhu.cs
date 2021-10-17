using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.NPCs.Bosses.BossReworks.EyeOfCthulhu.Minions
{
    public class ServantOfCthulhu : ModNPC
    {
        public bool dashing;
        public byte currentFrame;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 16;
            npc.height = 16;
            npc.damage = 15;
            npc.lifeMax = 3;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.friendly = false;

            dashing = false;
            currentFrame = 0;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 5;
            npc.defense = 0;
            npc.damage = 15;
            npc.knockBackResist = 0f;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage = 1;
            crit = false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 1;
            crit = false;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Rectangle frame = new Rectangle(0, (texture.Height / 2) * currentFrame, texture.Width, texture.Height / 2);

            spriteBatch.Draw(
                texture,
                npc.Center - Main.screenPosition,
                frame,
                drawColor,
                npc.rotation,
                new Vector2(10, 20),
                npc.scale,
                SpriteEffects.None,
                0f);
            return false;
        }

        // ai1 states
        // 0 = circle player phase 1
        // 1 = circle player phase 2
        // 2 = circle NPC

        public override void AI() //ai0 = NPC owner, ai1 = state
        {
            if (Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
                return;
            }

            if (Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<EyeOfCthulhu>() || !Main.npc[(int)npc.ai[0]].active)
            {
                npc.Kill(dropLoot: false);
                return;
            }

            int ringCount = 1;
            int myNum = -1;
            int multiply = 1;
            float addedX = 0f;
            float angleOffset = 0;
            Vector2 vectorToRotateAround = Vector2.Zero;

            npc.SmoothRotate(npc.DirectionTo(Main.player[npc.target].MountedCenter).ToRotation() - MathHelper.PiOver2, 0.05f);

            switch ((int)npc.ai[1])
            {
                case 0:
                case 1:
                    vectorToRotateAround = Main.player[npc.target].Center;

                    int maxRingCount = (int)npc.ai[1] == 1 ? 5 : 3;

                    if (npc.ai[1] == 1)
                        addedX += 20;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC other = Main.npc[i];

                        bool justChecked = false;
                        if (other.active && other.type == npc.type)
                        {
                            if (other.whoAmI == npc.whoAmI)
                            {
                                myNum = ringCount;
                                justChecked = true;
                            }

                            if (myNum == -1)
                            {
                                if (ringCount < maxRingCount)
                                {
                                    ringCount++;
                                }
                                else
                                {
                                    ringCount = 1;
                                    addedX += 40;
                                    multiply *= -1;
                                    angleOffset += MathHelper.PiOver2;
                                }
                            }
                            else
                            {
                                if (ringCount < maxRingCount && !justChecked)
                                {
                                    ringCount++;
                                }
                            }
                        }
                    }
                    break;

                case 2:
                    vectorToRotateAround = Main.npc[(int)npc.ai[0]].Center;
                    (myNum, ringCount) = npc.CountSameAsSelf(checkTarget: true);
                    addedX += 45;
                    break;
            }


            Vector2 position = vectorToRotateAround + new Vector2(0, -80 - addedX).RotatedBy((((MathHelper.TwoPi / ringCount) * myNum) + (Main.npc[(int)npc.ai[0]].modNPC as EyeOfCthulhu).currentRotation + angleOffset) * multiply);

            if (npc.DistanceSQ(position) > 5 * 5)
            {
                Vector2 velocity = position - npc.Center;
                velocity.Normalize();

                velocity *= 25f;
                npc.velocity = (npc.velocity * (25f - 1) + velocity) / 25f;
            }
            npc.velocity *= 0.95f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter > 30)
            {
                npc.frameCounter = 0;
                currentFrame++;

                if (currentFrame >= Main.npcFrameCount[npc.type])
                    currentFrame = 0;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (dashing)
                return base.CanHitPlayer(target, ref cooldownSlot);
            return false;
        }
    }
}
