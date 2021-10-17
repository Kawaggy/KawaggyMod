using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class BuffHelper
    {
        public static bool CheckProjectileExists(this Player player, int type, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[type] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                return true;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return false;
            }
        }
    }
}
