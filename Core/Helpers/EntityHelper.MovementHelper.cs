using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    /// <summary>
    /// Helper for various entities. <see cref="Item"/>s, <see cref="Player"/>s, <see cref="Projectile"/>s, <see cref="NPC"/>s and <see cref="Entity"/>s can use these
    /// </summary>
    public static partial class EntityHelper
    {
        public delegate void SpawnDust();
        /// <summary>
        /// Teleports the given <see cref="Entity"/> when it gets too far from a position
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to teleport</param>
        /// <param name="position">The position to teleport to</param>
        /// <param name="distance">The max distance</param>
        /// <param name="spawnDust">A way to add dust before and after it teleports</param>
        /// <returns>If it teleported</returns>
        public static bool TeleportIfTooFarFrom(this Entity entity, Vector2 position, float distance, SpawnDust spawnDust = null)
        {
            if (entity.DistanceSQ(position) > distance * distance)
            {
                if (spawnDust != null)
                {
                    spawnDust?.Invoke();
                }
                entity.position = position;
                entity.velocity *= 0.1f;
                if (spawnDust != null)
                {
                    spawnDust?.Invoke();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the given <see cref="Entity"/> to a given position
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to move</param>
        /// <param name="desiredPosition">The position to move to</param>
        /// <param name="minDistance">The minimum distance for it to move</param>
        /// <param name="speed">The speed</param>
        /// <param name="inertia">The inertia</param>
        public static void Move(this Entity entity, Vector2 desiredPosition, float minDistance, float speed, float inertia)
        {
            if (entity.DistanceSQ(desiredPosition) > minDistance * minDistance)
            {
                Vector2 velocity = desiredPosition - entity.Center;
                velocity.Normalize();
                velocity *= speed;
                entity.velocity = (entity.velocity * (inertia - 1) + velocity) / inertia;
            }
        }

        public static void DontOverlap(this Entity entity, float overlapSpeed)
        {
            if (entity is NPC npc)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC otherNPC = Main.npc[i];
                    if (i != npc.whoAmI && otherNPC.active && Math.Abs(npc.position.X - otherNPC.position.X) + Math.Abs(npc.position.Y - otherNPC.position.Y) < npc.width)
                    {
                        if (npc.position.X < otherNPC.position.X) npc.velocity.X -= overlapSpeed;
                        else npc.velocity.X += overlapSpeed;

                        if (npc.position.Y < otherNPC.position.Y) npc.velocity.Y -= overlapSpeed;
                        else npc.velocity.Y += overlapSpeed;
                    }
                }
            }

            if (entity is Projectile projectile)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile other = Main.projectile[i];
                    if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                    {
                        if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapSpeed;
                        else projectile.velocity.X += overlapSpeed;

                        if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapSpeed;
                        else projectile.velocity.Y += overlapSpeed;
                    }
                }
            }    
        }
    }
}
