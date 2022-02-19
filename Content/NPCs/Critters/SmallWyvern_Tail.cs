using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.NPCs.Critters
{
    //simplify this
    public class SmallWyvern_Tail : ModNPC
    {
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 250;
            npc.width = 12;
            npc.height = 12;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = false;
            npc.scale = 1f;
            npc.friendly = false;
        }

        public override string Texture => base.Texture + "_1";
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f && npc.ai[3] != 1)
                {
                    if (npc.ai[2] == 7)
                    {
                        npc.ai[0] = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height), ModContent.NPCType<SmallWyvern_Tail>(), npc.whoAmI, ai3: 1);
                        Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
                        Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] + 1;
                        Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
                        npc.netUpdate = true;
                    }
                }

                if (Main.npc[(int)npc.ai[1]].type != ModContent.NPCType<SmallWyvern_Body>() && Main.npc[(int)npc.ai[1]].type != ModContent.NPCType<SmallWyvern_Tail>())
                {
                    npc.Kill(false);
                }

                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.Kill(false);
                }

                if (npc.ai[2] == 7)
                {
                    if (!Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.Kill(false);
                    }

                    if (Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<SmallWyvern_Body>() && Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<SmallWyvern_Tail>())
                    {
                        npc.Kill(false);
                    }
                }

                if (!npc.active && Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f);
            }

            if (npc.ai[1] > 0 && npc.ai[1] < Main.maxNPCs)
            {
                Vector2 vectorToNext = Main.npc[(int)npc.ai[1]].Center - npc.Center;

                npc.rotation = (float)Math.Atan2(vectorToNext.Y, vectorToNext.X);
                float distance = (float)Math.Sqrt(vectorToNext.X * vectorToNext.X + vectorToNext.Y * vectorToNext.Y);
                int weirdFix = (int)(npc.width * 0.90);
                distance = (distance - weirdFix) / distance;
                vectorToNext *= distance;

                npc.velocity = Vector2.Zero;
                npc.position += vectorToNext;
            }

            npc.spriteDirection = npc.direction = Main.npc[(int)npc.ai[1]].spriteDirection;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Vector2 origin = new Vector2(texture.Width - 24, texture.Height / 2f);
            float rotation = npc.rotation;
            if (npc.ai[2] != 7)
            {
                rotation += MathHelper.Pi;
                origin = new Vector2(8, texture.Height / 2f);
                texture = ModContent.GetTexture("KawaggyMod/Content/NPCs/Critters/SmallWyvern_Tail_2");
            }

            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, rotation, origin, npc.scale, (SpriteEffects)npc.spriteDirection, 0f);
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
}
