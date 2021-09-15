using KawaggyMod.Content.Projectiles.KPlayer.Summoner;

namespace KawaggyMod.Core
{
    public static class CustomizationManager
    {
        public static IceSwordsCustomization iceSwords;

        public static void Load()
        {
            iceSwords = new IceSwordsCustomization();
            iceSwords.Load();
        }

        public static void Unload()
        {
            iceSwords.Unload();
            iceSwords = null;
        }
    }
}
