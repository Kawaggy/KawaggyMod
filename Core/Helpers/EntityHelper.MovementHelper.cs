using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class EntityHelper
    {
        /// <returns>If it teleported</returns>
        public static bool TeleportIfTooFarFrom(this Entity entity, Vector2 position, float distance)
        {
            if (entity.DistanceSQ(position) > distance * distance)
            {
                entity.position = position;
                entity.velocity *= 0.1f;
                return true;
            }
            return false;
        }
    }
}
