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

            KawaggyMod.Instance.Logger.Info("If you get many wrong format lines, delete the .txt's so that they get newly generated.");
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
