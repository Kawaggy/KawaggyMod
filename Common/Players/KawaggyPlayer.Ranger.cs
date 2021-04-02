using Terraria.ModLoader;

namespace KawaggyMod.Common.Players
{
    public partial class KawaggyPlayer : ModPlayer
    {
        public bool blueGelArrow;
        public bool pinkGelArrow;

        public void ResetRanger()
        {
            blueGelArrow = false;
            pinkGelArrow = false;
        }
    }
}
