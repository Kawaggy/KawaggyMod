using KawaggyMod.Core.DataTypes;
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
        
        public static PlayerData FindClosest<T>(this Entity entity, PlayerLogic playerLogic, bool needLineOfSight = false) where T : Player
        {
            Player closest = null;
            float distance = float.PositiveInfinity;
            bool lineOfSight = false;

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
                            bool tempLineOfSight = Collision.CanHitLine(entity.position, entity.width, entity.height, Main.player[i].position, Main.player[i].width, Main.player[i].height);
                            if (needLineOfSight && !tempLineOfSight)
                                continue;
                            lineOfSight = tempLineOfSight;
                            distance = newDistance;
                            closest = Main.player[i];
                        }
                    }
                }
            }

            return new PlayerData(closest, distance, lineOfSight);
        }

        public static NPCData FindClosest<T>(this Entity entity, NPCLogic npcLogic, bool needLineOfSight = false) where T : NPC
        {
            NPC closest = null;
            float distance = float.PositiveInfinity;
            bool lineOfSight = false;
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
                            bool tempLineOfSight = Collision.CanHitLine(entity.position, entity.width, entity.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height);
                            if (needLineOfSight && !tempLineOfSight)
                                continue;
                            lineOfSight = tempLineOfSight;
                            distance = newDistance;
                            closest = Main.npc[i];
                        }
                    }
                }
            }

            return new NPCData(closest, distance, lineOfSight);
        }

        public static ProjectileData FindClosest<T>(this Entity entity, ProjectileLogic projectileLogic, bool needLineOfSight = false) where T : Projectile
        {
            Projectile closest = null;
            float distance = float.PositiveInfinity;
            bool lineOfSight = false;
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
                            bool tempLineOfSight = Collision.CanHitLine(entity.position, entity.width, entity.height, Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height);
                            if (needLineOfSight && !lineOfSight)
                                continue;

                            lineOfSight = tempLineOfSight;
                            distance = newDistance;
                            closest = Main.projectile[i];
                        }
                    }
                }
            }

            return new ProjectileData(closest, distance, lineOfSight);
        }
    }
}
