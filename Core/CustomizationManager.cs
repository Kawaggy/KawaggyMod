using KawaggyMod.Content.Projectiles.KPlayer.Summoner;

namespace KawaggyMod.Core
{
    public static class CustomizationManager
    {
        public static IceSwordsCustomization iceSwords;
        public static CloudSummonCustomization cloudSummon;

        public static void Load()
        {
            iceSwords = new IceSwordsCustomization();
            iceSwords.Load();

            cloudSummon = new CloudSummonCustomization();
            cloudSummon.Load();
        }

        public static void Unload()
        {
            iceSwords.Unload();
            iceSwords = null;

            cloudSummon.Unload();
            cloudSummon = null;
        }
    }
}
