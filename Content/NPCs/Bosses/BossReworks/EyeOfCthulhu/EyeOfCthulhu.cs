using KawaggyMod.Content.NPCs.Bosses.BossReworks.EyeOfCthulhu.Minions;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.NPCs.Bosses.BossReworks.EyeOfCthulhu
{
    public class EyeOfCthulhu : ModNPC
    {
        public bool dashing;
        public byte currentFrame;
        public byte tonguedPlayer;
        public float currentRotation;

        public const int Hovering = 0;
        public const int Dashing = 1;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.aiStyle = -1;
            npc.width = 90;
            npc.height = 90;
            npc.damage = 15;
            npc.lifeMax = 2800;
            npc.defense = 12;
            npc.value = Item.sellPrice(gold: 3);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.friendly = false;

            //custom stuff
            dashing = false;
            currentFrame = 0;
            tonguedPlayer = byte.MaxValue;
            currentRotation = 0f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 3640 + (numPlayers * 420);
            npc.damage = 30;
            npc.value = Item.sellPrice(gold: 7, silver: 50) + (Item.sellPrice(gold: 1, silver: 50) * numPlayers);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(dashing);
            writer.Write(currentFrame);
            writer.Write(tonguedPlayer);
            writer.Write(currentRotation);
            writer.Write(npc.localAI[0]);
            writer.Write(npc.localAI[1]);
            writer.Write(npc.localAI[2]);
            writer.Write(npc.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            dashing = reader.ReadBoolean();
            currentFrame = reader.ReadByte();
            tonguedPlayer = reader.ReadByte();
            currentRotation = reader.ReadSingle();
            npc.localAI[0] = reader.ReadSingle();
            npc.localAI[1] = reader.ReadSingle();
            npc.localAI[2] = reader.ReadSingle();
            npc.localAI[3] = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Rectangle frame = new Rectangle(0, (texture.Height / 3) * currentFrame, texture.Width, texture.Height / 3);
            
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(
                texture,
                npc.Center + new Vector2(0, -30).RotatedBy(((MathHelper.TwoPi / 5) * i) + (currentRotation * 3)) - Main.screenPosition,
                frame,
                drawColor * 0.25f,
                npc.rotation,
                new Vector2(48, 57),
                npc.scale,
                SpriteEffects.None,
                0f);
            }
            

            spriteBatch.Draw(
                texture,
                npc.Center - Main.screenPosition,
                frame,
                drawColor,
                npc.rotation,
                new Vector2(48, 57),
                npc.scale,
                SpriteEffects.None,
                0f);

            Texture2D confusedTexture = ModContent.GetTexture("KawaggyMod/Assets/Extras/StarConfused");
            spriteBatch.Draw(confusedTexture, npc.Center + new Vector2(-20, -45).RotatedBy(npc.rotation) - Main.screenPosition, null, Color.White, -currentRotation * 2, confusedTexture.Size() / 2f, 1.5f + ((float)Math.Sin(currentRotation * 3) * 0.2f), SpriteEffects.None, 0);
            return false;
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

        public override void AI()
        {
            Player player = Main.player[npc.target];

            currentRotation += MathHelper.TwoPi / 360;
            currentRotation = MathHelper.WrapAngle(currentRotation);

            if (npc.localAI[3] == 0)
            {
                npc.TargetClosest();

                if (npc.DistanceSQ(Main.player[npc.target].Center) < 2000f)
                {
                    npc.localAI[3] = 1;
                }
            }

            switch(npc.ai[0])
            {
                case Hovering:
                    npc.CheckPlayerAlive();
                    npc.SmoothRotate(npc.DirectionTo(Main.player[npc.target].Center).ToRotation() - MathHelper.Pi, 0.05f);
                    npc.Move(player.Center + new Vector2(0, -250f), 15f, 8f, 24f);
                    npc.velocity *= 0.98f;
                    /*
                    if (++npc.ai[1] > 120 && npc.Count(type: ModContent.NPCType<ServantOfCthulhu>(), checkTarget: true) < 12)
                    {
                        NPC.NewNPC(X: (int)npc.Center.X, Y: (int)npc.Center.Y, Type: ModContent.NPCType<ServantOfCthulhu>(), ai0: npc.whoAmI, ai1: 2, Target:npc.target);
                        npc.ai[1] = 0;
                    }
                    */
                    break;
            }
        }

        public void NextAttack()
        {
            npc.TargetClosest();
            npc.ai[0] = Main.rand.Next(0, 2);
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;
            npc.localAI[0] = 0;
            npc.localAI[1] = 0;
            npc.localAI[2] = 0;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (dashing)
                return base.CanHitPlayer(target, ref cooldownSlot);
            return false;
        }
    }
}
