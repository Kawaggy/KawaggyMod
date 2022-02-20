using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Miscellaneous.Materials
{
    public class ForbiddenPebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Pebble");
            Tooltip.SetDefault("'Feels cursed...'");
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
