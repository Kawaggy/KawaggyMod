using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Helpers
{
    public static partial class BuffHelper
    {
        /// <summary>
        /// Checks if a given <see cref="Projectile"/> is active and owned by the <see cref="Player"/>
        /// </summary>
        /// <param name="player">The <see cref="Player"/></param>
        /// <param name="type">The type of the <see cref="Projectile"/></param>
        /// <param name="buffIndex">The index of the buff</param>
        /// <returns><see langword="true"/> if the projectile exists, keeping the buff, <see langword="false"/> otherwise, removing the buff</returns>
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

        /// <summary>
        /// Makes a certain mod buff a debuff
        /// </summary>
        /// <param name="buff">The buff to make into a debuff</param>
        public static void DebuffSetdefaults(this ModBuff buff)
        {
            buff.longerExpertDebuff = true;
            Main.debuff[buff.Type] = true;
            Main.pvpBuff[buff.Type] = true;
            Main.buffNoSave[buff.Type] = true;
        }
    }
}
