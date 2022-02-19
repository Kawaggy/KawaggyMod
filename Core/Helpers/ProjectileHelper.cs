using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static partial class ProjectileHelper
    {
        /// <summary>
        /// Terraria's projectile gravity
        /// </summary>
        public const float gravity = 0.4f;

        /// <summary>
        /// Smoothly rotates the <see cref="Projectile"/> to a given rotation
        /// </summary>
        /// <param name="projectile">The <see cref="Projectile"/></param>
        /// <param name="rotation">The rotation to go to (in radians)</param>
        /// <param name="strength">The strength</param>
        /// <returns>The amount added (in radians)</returns>
        public static float SmoothRotate(this Projectile projectile, float rotation, float strength = 0.2f, bool addRotation = true)
        {
            float addedRotation = MathHelper.WrapAngle(rotation - projectile.rotation) * strength;
            if (addRotation)
                projectile.rotation += addedRotation;
            return addedRotation;
        }

        /// <summary>
        /// Counts all <see cref="Projectile"/>s currently active and gets the count of the given <see cref="Projectile"/>
        /// </summary>
        /// <param name="projectile">The <see cref="Projectile"/> that wants to count the rest</param>
        /// <param name="checkOwner">If the count should check for the owner of the other <see cref="Projectile"/>s</param>
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

        /// <summary>
        /// Counts all avaiable <see cref="Projectile"/>s currently active
        /// </summary>
        /// <param name="type">The type of the <see cref="Projectile"/></param>
        /// <returns></returns>
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

        /// <summary>
        /// Counts all avaiable <see cref="Projectile"/>s currently active with a specified owner
        /// </summary>
        /// <param name="type">The type of the <see cref="Projectile"/></param>
        /// <param name="owner">The owner of the projectile</param>
        /// <returns></returns>
        public static int Count(int type, int owner)
        {
            return Main.player[owner].ownedProjectileCounts[type];
        }

        /// <summary>
        /// Checks if the <see cref="Projectile"/> is a summoner type
        /// </summary>
        /// <param name="projectile">The <see cref="Projectile"/> to check</param>
        /// <returns><see langword="true"/> if it is a summoner type, <see langword="false"/> otherwise</returns>
        public static bool IsSummon(this Projectile projectile)
        {
            return projectile.minion || projectile.sentry || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type];
        }

        /// <summary>
        /// Loops a <see cref="Projectile"/>'s animation
        /// </summary>
        /// <param name="projectile">The <see cref="Projectile"/> to animate</param>
        /// <param name="animationSpeed">The animation speed</param>
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
