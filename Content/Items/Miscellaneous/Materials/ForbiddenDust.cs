using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Materials
{
    public class ForbiddenDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Dust");
            Tooltip.SetDefault("'Feels blessed...'");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 14;
            item.value = Item.sellPrice(copper: 10);
            item.rare = ItemRarityID.White;
        }
    }
}
