using KawaggyMod.Core.ModTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Ranger
{
    public abstract class SniperBulletBase : ModExclusiveAcessory<SniperBulletBase>
    {
        internal bool gold;
        internal bool copper;

        public override string Texture => string.Join(string.Empty, "KawaggyMod/Content/Items/Accessories/Ranger/SniperBullet_", gold ? "Gold" : "Platinum", copper ? "Copper" : "Tin");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sniper Bullet");
            Tooltip.SetDefault("Increases ranged damage slightly");
        }

        public SniperBulletBase(bool gold, bool copper) : base()
        {
            this.gold = gold;
            this.copper = copper;
        }

        readonly int goldValue = Item.sellPrice(silver: 12);
        readonly int platinumValue = Item.sellPrice(silver: 18);
        readonly int copperValue = Item.sellPrice(silver: 1, copper: 50);
        readonly int tinValue = Item.sellPrice(silver: 2, copper: 25);

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 28;
            item.rare = ItemRarityID.Blue;
            item.value = (int)(((gold ? goldValue * 2 : platinumValue * 2) + (copper ? copperValue * 5 : tinValue * 5)) * 1.25f);
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            int item1 = gold ? ItemID.GoldBar : ItemID.PlatinumBar;
            int item2 = copper ? ItemID.CopperBar : ItemID.TinBar;

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(item1, 2);
            recipe.AddIngredient(item2, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.04f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            spriteBatch.Draw(texture, position + new Vector2(texture.Width, 0), null, Color.White, MathHelper.PiOver4, Vector2.Zero, scale, SpriteEffects.None, 0);
            return false;
        }
    }
    
    public class SniperBullet_GoldCopper : SniperBulletBase
    {
        public SniperBullet_GoldCopper() : base(true, true) { }
    }

    public class SniperBullet_GoldTin : SniperBulletBase
    {
        public SniperBullet_GoldTin() : base(true, false) { }
    }

    public class SniperBullet_PlatinumCopper : SniperBulletBase
    {
        public SniperBullet_PlatinumCopper() : base(false, true) { }
    }

    public class SniperBullet_PlatinumTin : SniperBulletBase
    {
        public SniperBullet_PlatinumTin() : base(false, false) { }
    }
}
