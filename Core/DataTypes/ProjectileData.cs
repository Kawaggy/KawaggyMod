using Terraria;

namespace KawaggyMod.Core.DataTypes
{
    public class ProjectileData
    {
        public Projectile projectile;
        public float distance;
        public bool hasLineOfSight;

        public ProjectileData(Projectile projectile, float distance, bool hasLineOfSight)
        {
            this.projectile = projectile;
            this.distance = distance;
            this.hasLineOfSight = hasLineOfSight;
        }
    }
}
