using KawaggyMod.Content.Buffs.Debuffs;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.GlobalNPCs
{
    public class DebuffGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool cursedFlames;
        public bool weakerIchor;

        public override void ResetEffects(NPC npc)
        {
            cursedFlames = false;
            weakerIchor = false;
        }

        public override void SetDefaults(NPC npc)
        {
            if (npc.buffImmune[BuffID.Ichor])
                npc.buffImmune[ModContent.BuffType<WeakerIchor>()] = true;
            if (npc.buffImmune[BuffID.CursedInferno])
                npc.buffImmune[ModContent.BuffType<CursedFlames>()] = true;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            int shownDamageValue = 0;

            SetShownDamageValue(npc, ref shownDamageValue);

            damage = shownDamageValue;
        }

        private void SetShownDamageValue(NPC npc, ref int shownDamageValue)
        {
            if (cursedFlames)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= CursedFlames.LifeLoss * 2;
                if (shownDamageValue < 1)
                    shownDamageValue = 1;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            ApplyDefenseDebuffs(npc, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ApplyDefenseDebuffs(npc, ref damage, ref knockback, ref crit);
        }

        private void ApplyDefenseDebuffs(NPC npc, ref int damage, ref float knockback, ref bool crit)
        {
            if (weakerIchor)
                damage = (int)(damage * (1f + (WeakerIchor.DefenseReduction / 100f)));
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (cursedFlames)
            {
                if (Main.rand.Next(10) < 5)
                {
                    npc.SpawnBuffDust(DustID.CursedTorch, npc.velocity, 100, 1.25f, 3.5f);
                }

                drawColor = Color.GreenYellow;
                Lighting.AddLight(npc.Center, Color.LightGreen.ToVector3() / 255f);
            }

            if (weakerIchor)
            {
                if (Main.rand.Next(10) < 5)
                {
                    npc.SpawnBuffDust(DustID.IchorTorch, npc.velocity, 100, 1.25f, 3.5f);
                }

                drawColor = Color.Yellow;
                Lighting.AddLight(npc.Center, Color.Yellow.ToVector3() / 255f);
            }
        }
    }
}
