using KawaggyMod.Common.Players;
using KawaggyMod.Content.Projectiles.KPlayer.Ranger;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Accessories.Ranger
{
    public class BlueGelArrow : KItem
    {
        public override string Texture => Assets.Items.Accessories + "Ranger/Arrow";
        private readonly string slimeTexture = Assets.Items.Accessories + "Ranger/BlueGelArrow_Overlay";

        public override bool Autoload(ref string name)
        {
            KawaggyPlayerEvents.ModifyHitNPCWithProjEvent += SpawnStickyArrow;
            return base.Autoload(ref name);
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.accessory = true;
            item.value = Item.sellPrice(silver: 3);
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Kawaggy().blueGelArrow = true;
        }

        private const float chance = 0.25f;
        private void SpawnStickyArrow(Player player, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (player.Kawaggy().blueGelArrow)
            {
                if (proj.arrow)
                {
                    if (proj.type != ModContent.ProjectileType<BlueGelArrowProj>())
                    {
                        if (Main.rand.NextFloat() < chance)
                        {
                            Vector2 speed = Vector2.Normalize(target.Center - player.Center) * 10f;
                            int type = ModContent.ProjectileType<BlueGelArrowProj>();
                            Projectile.NewProjectile(player.position, speed, type, proj.damage.RandomDamage(0.75f), 0f, player.whoAmI, 5.ToSeconds());
                        }
                    }
                }
            }
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.GetTexture(slimeTexture);
            Color color = drawColor;
            color.R = 145;
            color.G = 145;
            color.B = 145;
            color.A = 145;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture(slimeTexture);
            Color color = lightColor;
            color.R = 145;
            color.G = 145;
            color.B = 145;
            color.A = 145;
            spriteBatch.Draw(texture, new Vector2(
                item.position.X - Main.screenPosition.X + item.width * 0.5f,
                item.position.Y - Main.screenPosition.Y + item.height * 0.5f + 4f),
                new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 120);
            recipe.AddIngredient(ItemID.WoodenArrow, 90);
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(this);
        }
    }
}