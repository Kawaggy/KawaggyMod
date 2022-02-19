using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Materials
{
    public class BrainOfCthulhuTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain of Cthulhu Tooth");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 32;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 5);
            item.maxStack = 99;
        }
    }
}
