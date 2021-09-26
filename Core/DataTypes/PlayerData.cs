using Terraria;

namespace KawaggyMod.Core.DataTypes
{
    public class PlayerData
    {
        public Player player;
        public float distance;
        public bool hasLineOfSight;

        public PlayerData(Player player, float distance, bool hasLineOfSight)
        {
            this.player = player;
            this.distance = distance;
            this.hasLineOfSight = hasLineOfSight;
        }
    }
}
