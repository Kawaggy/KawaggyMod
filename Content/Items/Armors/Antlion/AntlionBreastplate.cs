using KawaggyMod.Content.Items.Miscellaneous.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Armors.Antlion
{
    [AutoloadEquip(EquipType.Body)]
    public class AntlionBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Antlion Breastplate");
            Tooltip.SetDefault("'It feels blessed...'" +
                "\n+4% magic damage and summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.defense = 4;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.04f;
            player.minionDamage += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 10);
            recipe.AddIngredient(ItemID.HardenedSand, 25);
            recipe.AddIngredient(ModContent.ItemType<ForbiddenDust>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
