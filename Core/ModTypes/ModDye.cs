using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace KawaggyMod.Core.ModTypes
{
    public abstract class ModDye : ModItem
    {
        public sealed override string Texture => "KawaggyMod/Assets/Items/DyeBottle";
        public override bool CloneNewInstances => true;
        public abstract string ShaderName { get; }
        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            byte dye = item.dye;
            SafeSetDefaults();
            item.width = 16;
            item.height = 24;
            item.dye = dye;
        }

        public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(ModContent.GetTexture(Texture), position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            DrawData data = new DrawData(ModContent.GetTexture("KawaggyMod/Assets/Items/DyeShader"), position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            GameShaders.Misc[ShaderName].Apply(data);
            data.Draw(spriteBatch);
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            return false;
        }

        public sealed override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(ModContent.GetTexture(Texture), item.Center - Main.screenPosition, ModContent.GetTexture(Texture).Frame(), lightColor, rotation, item.Size / 2f, scale, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            DrawData data = new DrawData(ModContent.GetTexture("KawaggyMod/Assets/Items/DyeShader"), item.Center - Main.screenPosition, ModContent.GetTexture("KawaggyMod/Assets/Items/DyeShader").Frame(), lightColor, rotation, item.Size / 2, scale, SpriteEffects.None, 0);
            GameShaders.Misc[ShaderName].Apply(data);
            data.Draw(spriteBatch);
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }
}
