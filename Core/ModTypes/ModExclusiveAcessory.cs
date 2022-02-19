using KawaggyMod.Common.Configs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

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
				DisplayName.SetDefault("{$" + commonName + "}");
            }

			if (commonTooltip != null)
            {
				Tooltip.SetDefault("{$" + commonTooltip + "}");
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

			(int index, Item accessory) = FindOtherAccessory();
			if (accessory != null)
			{
				int slot = -1;
				for (int i = 0; i < 10; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						int num3 = (int)(20f + (float)(i * 56) * Main.inventoryScale);
						int num4 = (int)(20f + (float)(j * 56) * Main.inventoryScale);
						if (Main.mouseX >= num3 && (float)Main.mouseX <= (float)num3 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num4 && (float)Main.mouseY <= (float)num4 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
						{
							slot = i + j * 10;
						}
					}
				}

				if (Main.mouseRight && Main.mouseRightRelease)
                {
					Utils.Swap(ref Main.LocalPlayer.inventory[slot], ref Main.LocalPlayer.armor[index]);
					Main.PlaySound(SoundID.Grab);
					Recipe.FindRecipes();
				}

				return false;
			}
			return base.CanRightClick();
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
