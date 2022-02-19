using KawaggyMod.Core.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Buffs.Debuffs
{
    public class CursedFlames : ModBuff
    {
        public const int LifeLoss = 3;

        public override void SetDefaults()
        {
            BuffHelper.DebuffSetdefaults(this);

            DisplayName.SetDefault("Cursed Flames");
            Description.SetDefault("Slowly losing life...");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Debuffs().cursedFlames = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Debuffs().cursedFlames = true;
        }
    }
}
