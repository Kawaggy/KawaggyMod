using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.DamageClasses.Developer
{
    public class DeveloperDamagePlayer : ModPlayer
    {
        public static DeveloperDamagePlayer Developer(Player player)
        {
            return player.GetModPlayer<DeveloperDamagePlayer>();
        }

        public float damageAdd;
        public float damageMult = 1f;
        public float knockback;
        public int crit;

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            damageAdd = 0f;
            damageMult = 0f;
            knockback = 0f;
            crit = 0;
        }
    }
}
