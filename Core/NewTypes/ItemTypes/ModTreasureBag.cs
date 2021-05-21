using KawaggyMod.Common.Worlds;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Core.NewTypes.ItemTypes
{
    public abstract class ModTreasureBag : ModBag
    {
        private readonly bool isExpert;
        private readonly int extraAttribute;

        public class ExtraAttribute
        {
            public const int None = 0;
            public const int Sadistic = 1;
            public const int Blessed = 2;
            public const int Cursed = 3;
        }

        public ModTreasureBag(int rare, bool isExpert, int extraAttribute = ExtraAttribute.None) : base(rare)
        {
            this.isExpert = isExpert;
            this.extraAttribute = extraAttribute;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            if (isExpert)
                item.expert = true;
            if (extraAttribute > ExtraAttribute.None)
                item.expert = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            switch(extraAttribute)
            {
                case ExtraAttribute.Sadistic:
                    ItemHelper.NewRarity(tooltips, new Color(120, 0, 0));
                    TooltipLine sadisticLine = new TooltipLine(mod, "SadisticText", Language.GetTextValue("Mods.KawaggyMod.Common.Sadistic"));
                    tooltips.Add(sadisticLine);
                    break;

                case ExtraAttribute.Blessed:
                    ItemHelper.NewRarity(tooltips, new Color(247, 255, 174));
                    TooltipLine blessedLine = new TooltipLine(mod, "BlessedText", Language.GetTextValue("Mods.KawaggyMod.Common.Blessed"));
                    tooltips.Add(blessedLine);
                    break;

                case ExtraAttribute.Cursed:
                    ItemHelper.NewRarity(tooltips, new Color(116, 86, 155));
                    TooltipLine cursedLine = new TooltipLine(mod, "CursedText", Language.GetTextValue("Mods.KawaggyMod.Common.Cursed"));
                    tooltips.Add(cursedLine);
                    break;

                case ExtraAttribute.None:
                default:
                    base.ModifyTooltips(tooltips);
                    break;
            }
        }

        private readonly string SadisticBagAura = Assets.Textures + "TreasureBagOutlines/SadisticTreasureBagOutline";
        private readonly string SadisticDustAura = Assets.Textures + "TreasureBagOutlines/SadisticTreasureBagOutline_Dust";

        private readonly string BlessedBagAura = Assets.Textures + "TreasureBagOutlines/BlessedTreasureBagOutline";
        private readonly string BlessedDustAura = Assets.Textures + "TreasureBagOutlines/BlessedTreasureBagOutline_Dust";

        private readonly string CursedBagAura = Assets.Textures + "TreasureBagOutlines/CursedTreasureBagOutline";
        private readonly string CursedDustAura = Assets.Textures + "TreasureBagOutlines/CursedTreasureBagOutline_Dust";

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            switch(extraAttribute)
            {
                case ExtraAttribute.Sadistic:
                    DrawAura(ModContent.GetTexture(SadisticBagAura), ModContent.GetTexture(SadisticDustAura), spriteBatch, rotation, scale);
                    break;
                case ExtraAttribute.Blessed:
                    DrawAura(ModContent.GetTexture(BlessedBagAura), ModContent.GetTexture(BlessedDustAura), spriteBatch, rotation, scale);
                    break;
                case ExtraAttribute.Cursed:
                    DrawAura(ModContent.GetTexture(CursedBagAura), ModContent.GetTexture(CursedDustAura), spriteBatch, rotation, scale);
                    break;
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        internal void DrawAura(Texture2D bag, Texture2D dust, SpriteBatch spriteBatch, float rotation, float scale)
        {
            spriteBatch.Draw(bag, new Vector2(
            item.position.X - Main.screenPosition.X + item.width * 0.5f,
            item.position.Y - Main.screenPosition.Y + item.height * 0.5f - 2f), new Rectangle(0, 0, bag.Width, bag.Height), Color.White, rotation, bag.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(dust, new Vector2(
            item.position.X - Main.screenPosition.X + item.width * 0.5f,
            item.position.Y - Main.screenPosition.Y + item.height * 0.5f - 8f), DustFrame, Color.White, rotation, new Vector2(26, 24), scale, SpriteEffects.None, 0f);
        }

        internal Rectangle DustFrame => new Rectangle(0, (240 / 5) * KawaggyWorld_Sadism.bagFrame, 52, 240 / 5);
    }
}
