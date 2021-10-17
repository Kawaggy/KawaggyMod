using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static class NPCHelper
    {
        public static float SmoothRotate(this NPC npc, float rotation, float strength = 0.2f)
        {
            float addedRotation = MathHelper.WrapAngle(rotation - npc.rotation) * strength;
            npc.rotation += addedRotation;
            npc.rotation = MathHelper.WrapAngle(npc.rotation);
            return addedRotation;
        }

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

        /// <returns>The npc' count / the entire count</returns>
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

        public static void Kill(this NPC npc, bool dropLoot)
        {
            npc.life = 0;
            npc.active = false;
            npc.HitEffect();

            if (dropLoot)
                npc.NPCLoot();
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
