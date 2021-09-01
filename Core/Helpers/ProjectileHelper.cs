using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class ProjectileHelper
    {
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
