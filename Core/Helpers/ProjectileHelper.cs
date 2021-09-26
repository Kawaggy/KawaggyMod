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

        public static int CountSameAsSelf(this Projectile projectile, bool checkOwner, bool countSelf = true)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].type == projectile.type)
                    {
                        if (checkOwner)
                        {
                            if (Main.projectile[i].owner == projectile.owner)
                            {
                                if (Main.projectile[i].whoAmI == projectile.whoAmI)
                                {
                                    if (countSelf)
                                        count++;
                                }
                                else
                                {
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            if (Main.projectile[i].whoAmI == projectile.whoAmI)
                            {
                                if (countSelf)
                                    count++;
                            }
                            else
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }
    }
}
