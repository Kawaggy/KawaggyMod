using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static class NPCHelper
    {
        /// <summary>
        /// Smoothly rotates the <see cref="NPC"/> to a given rotation
        /// </summary>
        /// <param name="npc">The <see cref="NPC"/></param>
        /// <param name="rotation">The rotation to go to (in radians)</param>
        /// <param name="strength">The strength</param>
        /// <returns>The amount added (in radians)</returns>
        public static float SmoothRotate(this NPC npc, float rotation, float strength = 0.2f)
        {
            float addedRotation = MathHelper.WrapAngle(rotation - npc.rotation) * strength;
            npc.rotation += addedRotation;
            npc.rotation = MathHelper.WrapAngle(npc.rotation);
            return addedRotation;
        }

        /// <summary>
        /// Checks if a <see cref="Player"/> is alive and in a given distance
        /// </summary>
        /// <param name="npc">The <see cref="NPC"/></param>
        /// <param name="maxDistance">The maximum distance allowed</param>
        /// <returns></returns>
        public static bool CheckPlayerAlive(this NPC npc, float maxDistance = 2500f)
        {
            npc.netUpdate = true;
            Player player = Main.player[npc.target];

            if (!player.active || player.dead || Vector2.DistanceSquared(npc.Center, player.Center) > maxDistance * maxDistance)
            {
                npc.TargetClosest();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Counts all <see cref="NPC"/>s currently active and gets the count of the given <see cref="NPC"/>
        /// </summary>
        /// <param name="npc">The <see cref="NPC"/> that wants to count the rest</param>
        /// <param name="checkTarget">If the count should check for the target of the other <see cref="NPC"/>s</param>
        /// <returns>The projectiles' count / the entire count</returns>
        public static (int whatIsMyCount, int myCount) CountSameAsSelf(this NPC npc, bool checkTarget)
        {
            int count = 0;
            int me = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                {
                    if (Main.npc[i].type == npc.type)
                    {
                        if (checkTarget)
                        {
                            if (Main.npc[i].target == npc.target)
                            {
                                count++;
                                if (Main.npc[i].whoAmI == npc.whoAmI)
                                {
                                    me = count;
                                }
                            }
                        }
                        else
                        {
                            count++;
                            if (Main.npc[i].whoAmI == npc.whoAmI)
                            {
                                me = count;
                            }
                        }
                    }
                }
            }

            return (me, count);
        }

        /// <summary>
        /// Kills a given <see cref="NPC"/>
        /// </summary>
        /// <param name="npc">The <see cref="NPC"/> to kill</param>
        /// <param name="dropLoot">If the <see cref="NPC"/> should drop loot</param>
        public static void Kill(this NPC npc, bool dropLoot)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.checkDead();
            npc.active = false;

            if (dropLoot)
                npc.NPCLoot();

            NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f);
        }

        public static int Count(this NPC npc, int type, bool checkTarget)
        {
            int count = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC other = Main.npc[i];

                if (other.active)
                {
                    if (other.type == type)
                    {
                        if (checkTarget)
                        {
                            if (npc.target == other.target)
                            {
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Checks if a boss is currently alive
        /// </summary>
        /// <returns><see langword="true"/> if a boss is currently alive, <see langword="false"/> otherwise</returns>
        public static bool BossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && (npc.boss || npc.type == NPCID.DD2Betsy))
                    return true;
            }
            return false;
        }
    }
}
