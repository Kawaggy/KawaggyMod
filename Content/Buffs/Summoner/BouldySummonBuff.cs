using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Buffs.Summoner
{
    public class BouldySummonBuff : KBuff
    {
        public override string Texture => Assets.Buffs.BuffsPath + "Summoner/BouldySummonBuff";

        public override void SetDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Bouldy>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
