using KawaggyMod.Content.Items.Miscellaneous.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Armors.Antlion
{
    [AutoloadEquip(EquipType.Legs)]
    public class AntlionLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Antlion Leggings");
            Tooltip.SetDefault("'It feels blessed...'" +
                "\n+2% magic damage and summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 14;
            item.defense = 2;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.02f;
            player.minionDamage += 0.02f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 8);
            recipe.AddIngredient(ItemID.HardenedSand, 18);
            recipe.AddIngredient(ModContent.ItemType<ForbiddenDust>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
