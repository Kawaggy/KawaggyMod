using Terraria.ModLoader;

namespace KawaggyMod.Common.Players
{
    public partial class KawaggyPlayer : ModPlayer
    {
        public bool blueGelCrown;
        public bool pinkGelCrown;
        public bool mahoganyAmulet;

        public void ResetSummoner()
        {
            blueGelCrown = false;
            pinkGelCrown = false;
            mahoganyAmulet = false;
        }
    }
}
