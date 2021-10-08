using KawaggyMod.Common.Configs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Core.ModTypes
{
	public abstract class ModExclusiveAcessory<T> : ModItem
	{
		internal string commonName;
		internal string commonTooltip;
		
		public ModExclusiveAcessory(string commonName, string commonTooltip)
        {
			this.commonName = commonName;
			this.commonTooltip = commonTooltip;
		}

		public ModExclusiveAcessory()
        {
			commonName = null;
			commonTooltip = null;
        }

        public override void SetStaticDefaults()
        {
			if (commonName != null)
            {
				DisplayName.SetDefault(commonName);
            }

			if (commonTooltip != null)
            {
				Tooltip.SetDefault(commonTooltip);
            }
        }

        public override void SetDefaults()
        {
			item.accessory = true;
			base.SetDefaults();
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
				int index = FindOtherAccessory().index;
				if (index != -1)
					return slot == index;
            }

            return base.CanEquipAccessory(player, slot);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			Item accessory = FindOtherAccessory().item;

			if (accessory != null && ModContent.GetInstance<DebugConfig>().ShowExtraInfo)
            {
				tooltips.Add(new TooltipLine(mod, "Swap" + nameof(T), Language.GetTextValue("Mods.KawaggyMod.Common.SwapWith", accessory.Name))
				{
					overrideColor = Color.Orange
				});
            }
        }

        public override bool CanRightClick()
        {
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 13; i < 13 + maxAccessoryIndex; i++)
			{
				if (Main.LocalPlayer.armor[i].type == item.type) return false;
			}

			if (FindOtherAccessory().item != null)
			{
				return true;
			}
			return base.CanRightClick();
		}

        public override void RightClick(Player player)
        {
			(int index, Item accessory) = FindOtherAccessory();
			if (accessory != null)
            {
				Main.LocalPlayer.QuickSpawnClonedItem(accessory);
				Main.LocalPlayer.armor[index] = item.Clone();
            }
        }

        public (int index, Item item) FindOtherAccessory()
        {
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 3; i < 3 + maxAccessoryIndex; i++)
			{
				Item otherAccessory = Main.LocalPlayer.armor[i];
				if (!otherAccessory.IsAir &&
					!item.IsTheSameAs(otherAccessory) &&
					otherAccessory.modItem is T)
				{
					return (i, otherAccessory);
				}
			}

			return (-1, null);
		}
    }
}
