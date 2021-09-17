using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    //Things that don't fit other specifications
    public static partial class EntityHelper
    {
        public delegate bool PlayerLogic(Player player);
        public delegate bool NPCLogic(NPC npc);
        public delegate bool ProjectileLogic(Projectile projectile);
        
        public static (int?, float) FindClosest<T>(this Entity entity, PlayerLogic playerLogic) where T : Player
        {
            int? closest = null;
            float distance = float.PositiveInfinity;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    bool? logicValue = true;

                    if (playerLogic != null)
                    {
                        logicValue = playerLogic?.Invoke(Main.player[i]);
                    }

                    if (logicValue ?? true)
                    {
                        float newDistance = entity.Distance(Main.player[i].Center);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closest = i;
                        }
                    }
                }
            }

            return (closest, distance);
        }

        public static (int?, float) FindClosest<T>(this Entity entity, NPCLogic npcLogic) where T : NPC
        {
            int? closest = null;
            float distance = float.PositiveInfinity;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type != NPCID.TargetDummy)
                {
                    bool? logicValue = true;

                    if (npcLogic != null)
                    {
                        logicValue = npcLogic?.Invoke(Main.npc[i]);
                    }

                    if (logicValue ?? true)
                    {
                        float newDistance = entity.Distance(Main.npc[i].Center);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closest = i;
                        }
                    }
                }
            }

            return (closest, distance);
        }

        public static (int?, float) FindClosest<T>(this Entity entity, ProjectileLogic projectileLogic) where T : Projectile
        {
            int? closest = null;
            float distance = float.PositiveInfinity;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active)
                {
                    bool? logicValue = true;

                    if (projectileLogic != null)
                    {
                        logicValue = projectileLogic?.Invoke(Main.projectile[i]);
                    }

                    if (logicValue ?? true)
                    {
                        float newDistance = entity.Distance(Main.projectile[i].Center);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closest = i;
                        }
                    }
                }
            }

            return (closest, distance);
        }
    }
}
