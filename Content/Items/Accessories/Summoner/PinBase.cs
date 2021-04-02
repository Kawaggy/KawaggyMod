using KawaggyMod.Core;

namespace KawaggyMod.Content.Items.Accessories.Summoner
{
    public class PinBase : KItem
    {
        public int value;
        public int rare;
        public PinBase(int value, int rare)
        {
            this.value = value;
            this.rare = rare;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 30;
            item.accessory = true;
            item.rare = rare;
            item.value = value;
        }
    }
}
