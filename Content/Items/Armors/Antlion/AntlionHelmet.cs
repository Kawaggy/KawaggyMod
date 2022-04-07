using KawaggyMod.Content.Items.Miscellaneous.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Armors.Antlion
{
    [AutoloadEquip(EquipType.Head)]
    public class AntlionHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Antlion Helmet");
            Tooltip.SetDefault("'It feels blessed...'" +
                "\n+2% magic damage and summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 14;
            item.defense = 3;
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.02f;
            player.minionDamage += 0.02f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AntlionBreastplate>() && legs.type == ModContent.ItemType<AntlionLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+4% magic and summon damage" +
                "\nAdditional minion slot";
            player.magicDamage += 0.04f;
            player.minionDamage += 0.04f;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 5);
            recipe.AddIngredient(ItemID.HardenedSand, 15);
            recipe.AddIngredient(ModContent.ItemType<ForbiddenDust>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
