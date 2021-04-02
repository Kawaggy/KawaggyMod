using KawaggyMod.Core.Helpers;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace KawaggyMod.Core.NewTypes.ItemTypes
{
    public abstract class ModBag : KItem
    {
        private readonly int Rare;

        public ModBag(int rare)
        {
            Rare = rare;
        }

        public virtual void SafeSetStaticDefaults() { }
        public sealed override void SetStaticDefaults()
        {
            SafeSetStaticDefaults();
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public virtual void SafeSetDefaults() { }
        public override void SetDefaults()
        {
            SafeSetDefaults();
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = Rare;
        }

        public override bool CanRightClick() => true;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ItemHelper.NewRarity(tooltips, Rare);
        }
    }

}
