using KawaggyMod.Content.Items.Miscellaneous.Dyes;
using KawaggyMod.Core.Helpers;
using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class Shaders
    {
        public static void Load(Mod mod)
        {
            KawaggyHelper.AddDye(ModContent.ItemType<BurningBloodDye>(), "BurningBlood", "BurningBloodPass");
            KawaggyHelper.AddDye(ModContent.ItemType<DeathWhisperDye>(), "DeathWhisper", "DeathWhisperPass");
        }
    }
}
