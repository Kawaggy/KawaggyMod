using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.NPCs.Critters
{
    public class SmallWyvern_Body : ModNPC
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

        public override string Texture => base.Texture + "_2";
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f)
                {
                    int variation;
                    int type;

                    switch (npc.ai[2])
                    {
                        case 3:
                            type = ModContent.NPCType<SmallWyvern_Body>();
                            variation = 1;
                            break;

                        case 4:
                            type = ModContent.NPCType<SmallWyvern_Body>();
                            variation = 2;
                            break;

                        case 5:
                            type = ModContent.NPCType<SmallWyvern_Body>();
                            variation = 3;
                            break;
                            
                        case 6:
                            type = ModContent.NPCType<SmallWyvern_Tail>();
                            variation = 0;
                            break;

                        default:
                            type = ModContent.NPCType<SmallWyvern_Body>();
                            variation = 0;
                            break;
                    }

                    npc.ai[0] = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height), type, npc.whoAmI, ai3: variation);
                    Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
                    Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] + 1;
                    Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
                    npc.netUpdate = true;
                }

                if (!Main.npc[(int)npc.ai[1]].active || !Main.npc[(int)npc.ai[0]].active)
                {
                    npc.Kill(false);
                }

                if (Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<SmallWyvern_Body>() && Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<SmallWyvern_Tail>())
                {
                    npc.Kill(false);
                }

                if (Main.npc[(int)npc.ai[1]].type != ModContent.NPCType<SmallWyvern_Head>() && Main.npc[(int)npc.ai[1]].type != ModContent.NPCType<SmallWyvern_Body>())
                {
                    npc.Kill(false);
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
            Vector2 origin = new Vector2(12, texture.Height / 2f);

            switch (npc.ai[3])
            {
                case 0:
                    texture = ModContent.GetTexture(Texture);
                    break;

                case 1:
                    texture = ModContent.GetTexture("KawaggyMod/Content/NPCs/Critters/SmallWyvern_Body_1");
                    break;

                case 2:
                    texture = ModContent.GetTexture("KawaggyMod/Content/NPCs/Critters/SmallWyvern_Body_3");
                    break;

                case 3:
                    texture = ModContent.GetTexture("KawaggyMod/Content/NPCs/Critters/SmallWyvern_Body_4");
                    break;
            }

            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, origin, npc.scale, (SpriteEffects)npc.spriteDirection, 0f);

            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }
}
