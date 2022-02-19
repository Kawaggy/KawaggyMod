using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.NPCs.Critters
{
    public class SmallWyvern_Head : ModNPC
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
            positionToGoTo = Vector2.Zero;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(positionToGoTo);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            positionToGoTo = reader.ReadVector2();
        }

        public Vector2 positionToGoTo;
        public override void AI()
        {
            npc.TargetClosest();

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f)
                {
                    npc.realLife = npc.whoAmI;
                    npc.ai[0] = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height), ModContent.NPCType<SmallWyvern_Body>(), npc.whoAmI, ai3: 1);
                    Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
                    Main.npc[(int)npc.ai[0]].ai[2] = 1;
                    Main.npc[(int)npc.ai[0]].realLife = npc.whoAmI;
                    positionToGoTo = Vector2.Zero;
                    npc.netUpdate = true;
                }
                
                if (!Main.npc[(int)npc.ai[0]].active)
                {
                    npc.Kill(false);
                }

                if (Main.npc[(int)npc.ai[0]].type != ModContent.NPCType<SmallWyvern_Body>())
                {
                    npc.Kill(false);
                }
                
                if (!npc.active && Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f);
            }
            bool lessThan45 = npc.life < npc.lifeMax * 0.45f;

            Player player = Main.player[npc.target];

            Vector2 vectorToPosition = player.Center - npc.Center;
            float distance = vectorToPosition.Length();

            float minVel = 0.5f;
            float maxVel = 2.75f;
            
            if (lessThan45)
            {
                if (positionToGoTo == Vector2.Zero)
                    positionToGoTo = new Vector2(Main.rand.Next(-320, 321), Main.rand.Next(-320, 321)) + npc.Center;
                vectorToPosition = positionToGoTo - npc.Center;
                distance = vectorToPosition.Length();
                if (distance < 30f)
                {
                    positionToGoTo = new Vector2(Main.rand.Next(-320, 321), Main.rand.Next(-320, 321)) + npc.Center;
                    npc.netUpdate = true;
                }
                minVel = 2.5f;
                maxVel = 4.5f;
            }
            
            if (distance > (lessThan45 ? 35f : 200f))
            {
                if (Math.Abs(vectorToPosition.X) > 10f)
                {
                    npc.velocity.X += minVel * (float)Math.Sign(vectorToPosition.X) * 0.35f;
                }
                if (Math.Abs(vectorToPosition.Y) > 5f)
                {
                    npc.velocity.Y += minVel * (float)Math.Sign(vectorToPosition.Y) * 0.35f;
                }
            }

            if (!lessThan45)
            {
                if (distance < 120f)
                {
                    if (Math.Abs(vectorToPosition.X) > 5f)
                    {
                        npc.velocity.X -= minVel * (float)Math.Sign(vectorToPosition.X) * 0.1f;
                    }
                    if (Math.Abs(vectorToPosition.Y) > 10f)
                    {
                        npc.velocity.Y -= minVel * (float)Math.Sign(vectorToPosition.Y) * 0.1f;
                    }
                }
            }

            else if (npc.velocity.Length() > 1.6f)
            {
                npc.velocity *= 0.96f;
            }

            if (npc.velocity.Length() > maxVel)
            {
                npc.velocity = Vector2.Normalize(npc.velocity) * maxVel;
            }

            if (Math.Abs(npc.velocity.Y) < 1f)
            {
                npc.velocity.Y -= 0.1f;
            }

            npc.rotation = npc.velocity.ToRotation() + MathHelper.Pi;

            int oldDirection = npc.direction;
            npc.direction = npc.spriteDirection = (npc.velocity.X > 0f).ToDirectionInt();

            if (oldDirection != npc.direction)
            {
                npc.netUpdate = true;
            }

            npc.position = npc.Center;
            npc.width = npc.height = 12;
            npc.Center = npc.position;
        }

        //make ai better before letting it spawn
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            /*
            if (spawnInfo.sky)
            {
                if (Main.hardMode)
                    return 0.05f;
                return 0.25f;
            }
            */
            return 0f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Vector2 origin = new Vector2(8, texture.Height / 2f);
            spriteBatch.Draw(Main.magicPixel, positionToGoTo - Main.screenPosition, new Rectangle(0, 0, 4, 4), Color.Red, 0f, new Vector2(2), 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(Main.magicPixel, positionToGoTo - Main.screenPosition, new Rectangle(0, 0, 35, 35), Color.Red * 0.25f, 0f, new Vector2(17.5f), 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(Main.magicPixel, positionToGoTo - Main.screenPosition, new Rectangle(0, 0, 30, 30), Color.Blue * 0.25f, 0f, new Vector2(15f), 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation + MathHelper.Pi, origin, npc.scale, (SpriteEffects)npc.spriteDirection, 0f);
            return false;
        }
    }
}
