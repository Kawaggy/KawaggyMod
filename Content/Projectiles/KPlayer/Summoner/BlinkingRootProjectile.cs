using KawaggyMod.Core.DataTypes;
using KawaggyMod.Core.Helpers;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class BlinkingRootProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            positionToTeleportTo = Vector2.Zero;
        }

        public Vector2 positionToTeleportTo;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            NPCData data = projectile.FindClosest<NPC>(delegate(NPC npc) { return true; }, true);

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.active && projectile.DistanceSQ(npc.Center) < 800f * 800f)
                {
                    data.npc = npc;
                    data.distance = projectile.Distance(npc.Center);
                    data.hasLineOfSight = Collision.CanHitLine(projectile.Center, projectile.width, projectile.height, npc.Center, npc.width, npc.height);
                }
            }

            if (projectile.ai[0] == 0f)
                projectile.ai[0] = 500f;

            //this is the teleporting logic
            //spoiler: there is none!
            if (positionToTeleportTo.X != 0f && positionToTeleportTo.Y != 0f)
            {
                projectile.position = positionToTeleportTo * 16f;
                projectile.velocity = Vector2.Zero;
                positionToTeleportTo = Vector2.Zero;
            }

            projectile.ai[0]++;
            if (projectile.ai[0] == 100f || projectile.ai[0] == 200f || projectile.ai[0] == 300f)
            {
                projectile.ai[1] = 30f;
                projectile.netUpdate = true;
            }

            if (projectile.ai[0] >= 650f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                projectile.ai[0] = 1f;
                SetTeleportPosition(data == null ? player.Center : data.npc.Center);
            }
        }

        public void SetTeleportPosition(Vector2 position)
        {
            Vector2 entityPosition = position.ToWorldCoordinates(0, 0);
            Vector2 projectilePoint = projectile.position.ToWorldCoordinates(0, 0);

            int tilesToCheck = 20;
            int tries = 0;
            bool canGetTeleportPoint = true;

            if (Math.Abs(projectile.position.X - position.X) + Math.Abs(projectile.position.Y - position.Y) > 2000f)
            {
                tries = 100;
                canGetTeleportPoint = false;
            }

            while (canGetTeleportPoint && tries < 100)
            {
                tries++;

                int randX = Main.rand.Next((int)entityPosition.X - tilesToCheck, (int)entityPosition.X + tilesToCheck);
                int randY = Main.rand.Next((int)entityPosition.Y - tilesToCheck, (int)entityPosition.Y + tilesToCheck);
                for (int i = randY; i < (int)entityPosition.Y + tilesToCheck; i++)
                {
                    if ((i < (int)entityPosition.Y - 4 || i > (int)entityPosition.Y + 4 || randX < (int)entityPosition.X - 4 || randX > (int)entityPosition.X + 4) && (i < (int)projectilePoint.Y - 1 || i > (int)projectilePoint.Y + 1 || randX < (int)projectilePoint.X - 1 || randX > (int)projectilePoint.X + 1) && Main.tile[randX, i].nactive())
                    {
                        bool flag5 = true;
                        if (Main.tile[randX, i - 1].lava())
                            flag5 = false;

                        if (flag5 && Main.tileSolid[Main.tile[randX, i].type] && !Collision.SolidTiles(randX - 1, randX + 1, i - 4, i - 1))
                        {
                            projectile.ai[1] = 20f;
                            positionToTeleportTo = new Vector2(randX, i);
                            canGetTeleportPoint = false;
                            break;
                        }
                    }
                }
            }

            projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(positionToTeleportTo);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            positionToTeleportTo = reader.ReadVector2();
        }
    }
}
