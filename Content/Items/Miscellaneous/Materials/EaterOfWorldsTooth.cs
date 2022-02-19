using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Materials
{
    public class EaterOfWorldsTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eater of Worlds Tooth");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 32;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 3, copper:50);
            item.maxStack = 99;
        }
    }
}
