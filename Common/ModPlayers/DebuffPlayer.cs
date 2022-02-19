using KawaggyMod.Content.Buffs.Debuffs;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    public class DebuffPlayer : ModPlayer
    {
        public bool cursedFlames;
        public bool weakerIchor;

        public override void ResetEffects()
        {
            cursedFlames = false;
            weakerIchor = false;
        }

        public override void UpdateDead()
        {
            cursedFlames = false;
            weakerIchor = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (weakerIchor)
            {
                player.statDefense -= WeakerIchor.DefenseReduction;
            }

            if (player.statDefense < 0)
                player.statDefense = 0;
        }

        public override void UpdateBadLifeRegen()
        {
            int amount = 0;
            float multiplication = 1f;
            SetAmount(ref amount);
            SetMultiplication(ref multiplication);

            player.lifeRegen -= (int)((amount * 2) * multiplication);
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (cursedFlames)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    Main.playerDrawDust.Add(player.SpawnBuffDust(drawInfo.position ,DustID.CursedTorch, player.velocity * 0.4f, 100, 3f));
                }
                r *= Color.YellowGreen.R / 255f;
                g *= Color.YellowGreen.G / 255f;
                b *= Color.YellowGreen.B / 255f;
                fullBright = true;
            }

            if (weakerIchor)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    Main.playerDrawDust.Add(player.SpawnBuffDust(drawInfo.position, DustID.IchorTorch, player.velocity * 0.4f, 100, 3f));
                }
                r *= Color.Yellow.R / 255f;
                g *= Color.Yellow.G / 255f;
                b *= Color.Yellow.B / 255f;
                fullBright = true;
            }
        }

        private void SetAmount(ref int amount)
        {
            if (cursedFlames)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 0;
                amount += CursedFlames.LifeLoss;
            }
        }

        private void SetMultiplication(ref float multiplication)
        {

        }
    }
}
