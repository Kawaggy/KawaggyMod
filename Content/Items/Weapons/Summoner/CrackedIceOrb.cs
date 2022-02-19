using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Content.Items.Miscellaneous;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Summoner
{
    public class CrackedIceOrb : ModItem, ICustomizable
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;

            DisplayName.SetDefault("Cracked Ice Orb");
            Tooltip.SetDefault("Summons a magical flying Ice Sword to protect you");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.damage = 24;
            item.knockBack = 3f;
            item.mana = 10;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(silver: 40);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<IceSwordsBuff>();
            item.shoot = ModContent.ProjectileType<IceSwords>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.Bone, 5);
            recipe.AddIngredient(ModContent.ItemType<ChillingShard>(), 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;

            Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI)].frame = Main.rand.Next(0, CustomizationManager.iceSwords.FrameCount);
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.GetTexture(Texture + "_Shine");
            spriteBatch.Draw(texture, position, new Rectangle?(ShineFrame()), Color.White * 0.75f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture(Texture + "_Shine");
            spriteBatch.Draw(texture, item.position + (item.Size / 2f) - Main.screenPosition, new Rectangle?(ShineFrame()), lightColor * 0.75f, rotation, item.Size / 2f, scale, SpriteEffects.None, 0f);
        }

        int frame;
        int counter;

        public int Count => CustomizationManager.iceSwords.cache.Count;

        internal Rectangle ShineFrame()
        {
            if (Main.rand.Next(100) == 0 && counter == 0)
            {
                frame = 0;
                counter++;
            }

            if (counter > 0)
                counter++;

            if (counter % 10 == 0)
                frame++;

            if (frame == 3)
            {
                counter = 0;
                frame = 40;
            }
            return new Rectangle(0, item.height * frame, item.width, item.height);
        }
    }
}
