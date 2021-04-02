using KawaggyMod.Common.Players;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Summoner
{
    public class PinkGelCrown : KItem
    {
        public override bool Autoload(ref string name)
        {
            KawaggyPlayerEvents.ModifyHitNPCWithProjEvent += StealLife;
            return base.Autoload(ref name);
        }

        public override string Texture => Assets.Items.Accessories + "Summoner/PinkGelCrown";

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.accessory = true;
            item.value = Item.sellPrice(silver: 26, copper: 20);
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Kawaggy().pinkGelCrown = true;
        }

        const float chance = 0.20f;
        const float stealPercentage = 0.25f;
        private void StealLife(Player player, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
            {
                if (player.Kawaggy().pinkGelCrown)
                {
                    if (Main.rand.NextFloat() < chance)
                    {
                        player.StealLife(damage, stealPercentage);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PinkGel, 30);
            recipe.AddRecipeGroup("KawaggyMod:AnyCrown");
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
