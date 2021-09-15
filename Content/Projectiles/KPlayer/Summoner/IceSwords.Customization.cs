using KawaggyMod.Core;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class IceSwordsCustomization : CustomizationBase
    {
        public IceSwordsCustomization() : base("IceSwords", 18, 44) { }

        public override int FrameCount
        {
            get
            {
                return cache.Count + 5;
            }
            set
            {
            }
        }
    }
}
