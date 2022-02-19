using KawaggyMod.Core.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Buffs.Debuffs
{
    public class WeakerIchor : ModBuff
    {
        public const int DefenseReduction = 5;

        public override void SetDefaults()
        {
            BuffHelper.DebuffSetdefaults(this);

            DisplayName.SetDefault("Weaker Ichor");
            Description.SetDefault("Defense slightly reduced");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Debuffs().weakerIchor = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Debuffs().weakerIchor = true;
        }
    }
}
