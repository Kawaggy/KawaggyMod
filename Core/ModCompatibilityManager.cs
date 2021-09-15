using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class ModCompatibilityManager
    {
        public static Mod junkoAndFriends;

        public static void Load()
        {
            junkoAndFriends = ModLoader.GetMod("JunkoAndFriends");
        }

        public static void Unload()
        {
            junkoAndFriends = null;
        }
    }
}
