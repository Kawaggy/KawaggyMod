using KawaggyMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Ranger
{
    public abstract class SniperBulletsBase : KItem
    {
        public struct BulletInfo
        {
            public int value;
            public int item1;
            public int item2;

            public BulletInfo(int value, int item1, int item2)
            {
                this.value = value;
                this.item1 = item1;
                this.item2 = item2;
            }
        }
        
        BulletInfo info;
        readonly string texturePath;

        public SniperBulletsBase(BulletInfo info, string texturePath) { this.info = info; this.texturePath = texturePath; }
        public override string Texture => texturePath == "" ? base.Texture : texturePath;

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 28;
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.value = info.value;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(info.item1, 2);
            recipe.AddIngredient(info.item2, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class SniperBullet_GoldCopper : SniperBulletsBase
    {
        public SniperBullet_GoldCopper() : base(new BulletInfo(Item.sellPrice(silver: 31, copper: 50), ItemID.GoldBar, ItemID.CopperBar), Assets.Items.Accessories + "Ranger/SniperBullet_GoldCopper") { }
    }

    public class SniperBullet_GoldTin : SniperBulletsBase
    {
        public SniperBullet_GoldTin() : base(new BulletInfo(Item.sellPrice(silver: 35, copper: 25), ItemID.GoldBar, ItemID.TinBar), Assets.Items.Accessories + "Ranger/SniperBullet_GoldTin") { }
    }

    public class SniperBullet_PlatinumCopper : SniperBulletsBase
    {
        public SniperBullet_PlatinumCopper() : base(new BulletInfo(Item.sellPrice(silver: 43, copper: 50), ItemID.PlatinumBar, ItemID.CopperBar), Assets.Items.Accessories + "Ranger/SniperBullet_PlatinumCopper") { }
    }

    public class SniperBullet_PlatinumTin : SniperBulletsBase
    {
        public SniperBullet_PlatinumTin() : base(new BulletInfo(Item.sellPrice(silver: 47, copper: 25), ItemID.PlatinumBar, ItemID.TinBar), Assets.Items.Accessories + "Ranger/SniperBullet_PlatinumTin") { }
    }
}
