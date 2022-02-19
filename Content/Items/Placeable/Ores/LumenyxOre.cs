using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Placeable.Ores
{
    public class LumenyxOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
            DisplayName.SetDefault("Lumenyx Ore");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 999;
            //item.consumable = true;
            //item.createTile = ModContent.TileType<>();
            item.width = 10;
            item.height = 10;
            item.noUseGraphic = true;
        }

        public override string Texture => "KawaggyMod/Content/Items/Placeable/Ores/LumenyxOre_Stone";

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Main.DiscoColor.ToVector3());

            if (Main.rand.Next(50) == 0)
            {
                Dust.NewDust(item.position - new Vector2(0, 20), 16, 16, DustID.Sparkle, newColor: Main.DiscoColor);
            }
        }

        //This? This is pain.
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D gemTexture = ModContent.GetTexture("KawaggyMod/Content/Items/Placeable/Ores/LumenyxOre_Gem");
            Texture2D rockTexture = ModContent.GetTexture("KawaggyMod/Content/Items/Placeable/Ores/LumenyxOre_Rocks");
            spriteBatch.Draw(gemTexture, position, frame, Main.DiscoColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(rockTexture, position + new Vector2(18, (gemTexture.Height / 2f) - 6 - 3 * ((float)Math.Sin(Main.GlobalTime))), new Rectangle(0, 0, 8, 10), drawColor, 0f, new Vector2(4, 5), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(rockTexture, position + new Vector2(-4, (gemTexture.Height / 2f) + 8 - 6 - 3 * ((float)Math.Sin(Main.GlobalTime + MathHelper.PiOver2))), new Rectangle(10, 0, 6, 6), drawColor, 0f, new Vector2(3, 3), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(rockTexture, position + new Vector2(-2, (gemTexture.Height / 2f) - 6 - 6 - 3 * ((float)Math.Sin(Main.GlobalTime + MathHelper.PiOver4))), new Rectangle(18, 0, 4, 4), drawColor, 0f, new Vector2(2, 2), scale, SpriteEffects.None, 0);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture("KawaggyMod/Content/Items/Placeable/Ores/LumenyxOre_Gem");
            Vector2 offset = new Vector2(item.width / 2 - texture.Width / 2, item.height - texture.Height + 2f);
            Vector2 origin = texture.Size() / 2f;
            spriteBatch.Draw(texture, item.position - Main.screenPosition + origin + offset, null, Main.DiscoColor, rotation, origin, scale, SpriteEffects.None, 0f);

            Texture2D rockTexture = ModContent.GetTexture("KawaggyMod/Content/Items/Placeable/Ores/LumenyxOre_Rocks");
            spriteBatch.Draw(rockTexture, item.position - Main.screenPosition + origin + offset + new Vector2(15, -3 * ((float)Math.Sin(Main.GlobalTime))), new Rectangle(0, 0, 8, 10), lightColor, rotation, new Vector2(4, 5), scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(rockTexture, item.position - Main.screenPosition + origin + offset + new Vector2(-12, 8 -3 * ((float)Math.Sin(Main.GlobalTime + MathHelper.PiOver2))), new Rectangle(10, 0, 6, 6), lightColor, rotation, new Vector2(3, 3), scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(rockTexture, item.position - Main.screenPosition + origin + offset + new Vector2(-10, -8 -3 * ((float)Math.Sin(Main.GlobalTime + MathHelper.PiOver4))), new Rectangle(18, 0, 4, 4), lightColor, rotation, new Vector2(2, 2), scale, SpriteEffects.None, 0f);
        }
    }
}
