using KawaggyMod.Core;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class CloudSummonCustomization : CustomizationBase
    {
        public CloudSummonCustomization() : base("CloudSummon", 32, 32) { }
        public override int FrameCount
        {
            get
            {
                return cache.Count + 11;
            }
        }
    }
}
