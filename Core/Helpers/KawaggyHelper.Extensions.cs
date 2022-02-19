using KawaggyMod.Common.GlobalNPCs;
using KawaggyMod.Common.ModPlayers;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static SummonPlayer Summons(this Player player) => player.GetModPlayer<SummonPlayer>();
        public static KawaggyPlayer Kawaggy(this Player player) => player.GetModPlayer<KawaggyPlayer>();
        public static DebuffPlayer Debuffs(this Player player) => player.GetModPlayer<DebuffPlayer>();
        public static CameraPlayer Camera(this Player player) => player.GetModPlayer<CameraPlayer>();
        public static DebuffGlobalNPC Debuffs(this NPC npc) => npc.GetGlobalNPC<DebuffGlobalNPC>();
    }
}
