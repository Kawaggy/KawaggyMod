using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static partial class ProjectileHelper
    {
        public const float gravity = 0.4f;

        /// <returns>The amount added</returns>
        public static float SmoothRotate(this Projectile projectile, float rotation, float strength = 0.2f)
        {
            float addedRotation = MathHelper.WrapAngle(rotation - projectile.rotation) * strength;
            projectile.rotation += addedRotation;
            return addedRotation;
        }

        /// <returns>The projectiles' count / the entire count</returns>
        public static (int whatIsMyCount, int myCount) CountSameAsSelf(this Projectile projectile, bool checkOwner)
        {
            int count = 0;
            int me = -1;
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
                                count++;
                                if (Main.projectile[i].whoAmI == projectile.whoAmI)
                                {
                                    me = count;
                                }
                            }
                        }
                        else
                        {
                            count++;
                            if (Main.projectile[i].whoAmI == projectile.whoAmI)
                            {
                                me = count;
                            }
                        }
                    }
                }
            }

            return (me, count);
        }

        public static int Count(int type)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type)
                {
                    count++;
                }
            }

            return count;
        }

        public static bool IsSummon(this Projectile projectile)
        {
            return projectile.minion || projectile.sentry || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type];
        }

        public static void SimpleAnimation(this Projectile projectile, int animationSpeed)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > animationSpeed)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > Main.projFrames[projectile.type] - 1)
                    projectile.frame = 0;
            }
        }
    }
}
