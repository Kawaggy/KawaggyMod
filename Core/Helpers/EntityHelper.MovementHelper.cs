using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class EntityHelper
    {
        public delegate void SpawnDust();
        /// <summary>
        /// Teleports the given entity when it gets too far from a position
        /// </summary>
        /// <param name="entity">The entity to teleport</param>
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
    }
}
