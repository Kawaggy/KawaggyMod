using KawaggyMod.Common.ModPlayers;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static SummonPlayer Summons(this Player player) => player.GetModPlayer<SummonPlayer>();
        public static KawaggyPlayer Kawaggy(this Player player) => player.GetModPlayer<KawaggyPlayer>();
    }
}
