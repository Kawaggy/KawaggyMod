using KawaggyMod.Core.DataTypes;
using Microsoft.Xna.Framework;
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

        /// <summary>
        /// Resizes an entity to a new width and a new height
        /// </summary>
        /// <param name="entity">The entity to be resized</param>
        /// <param name="newWidth">The new width</param>
        /// <param name="newHeight">The new height</param>
        public static void Resize(this Entity entity, int newWidth, int newHeight)
        {
            entity.position = entity.Center;
            entity.width = newWidth;
            entity.height = newHeight;
            entity.Center = entity.position;
        }

        /// <summary>
        /// Resizes an entity to a new size
        /// </summary>
        /// <param name="entity">The entity to be resized</param>
        /// <param name="newSize">The new size</param>
        public static void Resize(this Entity entity, int newSize)
        {
            entity.position = entity.Center;
            entity.width = newSize;
            entity.height = newSize;
            entity.Center = entity.position;
        }

        /// <summary>
        /// Spawns dust around an entity
        /// </summary>
        /// <param name="entity">The entity to spawn dust on</param>
        /// <param name="dustType">The dust type</param>
        /// <param name="velocity">The dust velocity</param>
        /// <param name="alpha">The alpha of the projectile</param>
        /// <param name="minSize">The minimum size</param>
        /// <param name="maxSize">The maximum size</param>
        /// <returns>The dust whoAmI</returns>
        public static int SpawnBuffDust(this Entity entity, int dustType, Vector2 velocity, int alpha, float minSize, float maxSize = -1f)
        {
            float size = minSize;
            if (maxSize != -1f)
                size = Main.rand.NextFloat(minSize, maxSize);

            int dust = Dust.NewDust(entity.position - new Vector2(2f), entity.width + 4, entity.height + 4, dustType, velocity.X, velocity.Y, alpha, Scale: size);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1.8f;
            Main.dust[dust].velocity.Y -= 0.5f;
            if (Main.rand.NextBool(4))
            {
                Main.dust[dust].noGravity = false;
                Main.dust[dust].scale *= 0.5f;
            }

            return dust;
        }

        /// <summary>
        /// Spawns dust around an entity
        /// </summary>
        /// <param name="entity">The entity to spawn dust on</param>
        /// <param name="position">The position to spawn the dust on</param>
        /// <param name="dustType">The dust type</param>
        /// <param name="velocity">The dust velocity</param>
        /// <param name="alpha">The alpha of the projectile</param>
        /// <param name="minSize">The minimum size</param>
        /// <param name="maxSize">The maximum size</param>
        /// <returns>The dust whoAmI</returns>
        public static int SpawnBuffDust(this Entity entity, Vector2 position, int dustType, Vector2 velocity, int alpha, float minSize, float maxSize = -1f)
        {
            float size = minSize;
            if (maxSize != -1f)
                size = Main.rand.NextFloat(minSize, maxSize);

            int dust = Dust.NewDust(position - new Vector2(2f), entity.width + 4, entity.height + 4, dustType, velocity.X, velocity.Y, alpha, Scale: size);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1.8f;
            Main.dust[dust].velocity.Y -= 0.5f;
            if (Main.rand.NextBool(4))
            {
                Main.dust[dust].noGravity = false;
                Main.dust[dust].scale *= 0.5f;
            }

            return dust;
        }
    }
}
