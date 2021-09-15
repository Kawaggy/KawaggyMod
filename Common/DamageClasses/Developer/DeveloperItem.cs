using KawaggyMod.Core.Interfaces;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Common.DamageClasses.Developer
{
    public abstract class DeveloperItem : ModItem, ICustomRarity
    {
        public override bool CloneNewInstances => true;
        public abstract bool Male { get; }
        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
            SafeSetDefaults();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            add += DeveloperDamagePlayer.Developer(player).damageAdd;
            mult += DeveloperDamagePlayer.Developer(player).damageMult;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            knockback += DeveloperDamagePlayer.Developer(player).knockback;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit += DeveloperDamagePlayer.Developer(player).crit;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach(TooltipLine line in tooltips)
            {
                if (line.mod == "Terraria" && line.Name == "Damage")
                {
                    string[] splitText = line.text.Split(' ');
                    string damageValue = splitText[0];
                    string damageWord = splitText[splitText.Length];

                    line.text = damageValue + Language.GetTextValue(Male ? "Mods.KawaggyMod.Common.DeveloperMale" : "Mods.KawaggyMod.Common.DeveloperFemale") + damageWord;
                }
            }
        }
    }
}
