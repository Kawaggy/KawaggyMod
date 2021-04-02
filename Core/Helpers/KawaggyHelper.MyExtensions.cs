using Terraria;
using KawaggyMod.Common.Players;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static KawaggyPlayer Kawaggy(this Player player) => player.GetModPlayer<KawaggyPlayer>();
    }
}
