using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class ProjectileHelper
    {
        public static bool TeleportIfTooFarFrom(this Projectile projectile, Vector2 position, float distance)
        {
            if (projectile.DistanceSQ(position) > distance * distance)
            {
                projectile.position = position;
                projectile.velocity *= 0.1f;
                return true;
            }
            return false;
        }

        /// <returns>The amount added</returns>
        public static float SmoothRotate(this Projectile projectile, float rotation, float strength)
        {
            float addedRotation = MathHelper.WrapAngle(rotation - projectile.rotation) * strength;
            projectile.rotation += addedRotation;
            return addedRotation;
        }

        public const float gravity = 0.4f;
    }
}
