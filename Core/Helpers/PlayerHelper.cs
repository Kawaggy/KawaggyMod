using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class PlayerHelper
    {
        public static bool AllJumpsDepleted(this Player player)
        {
            return !player.jumpAgainBlizzard && !player.jumpAgainCloud && !player.jumpAgainFart && !player.jumpAgainSail && !player.jumpAgainSandstorm && !player.jumpAgainUnicorn;
        }

        public static bool IsFlying(this Player player)
        {
            bool flying = false;

            if (player.wingsLogic > 0 && player.controlJump && player.wingTime > 0f && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f)
                flying = true;

            if ((player.wingsLogic == 22 || player.wingsLogic == 28 || player.wingsLogic == 30 || player.wingsLogic == 32 || player.wingsLogic == 29 || player.wingsLogic == 33 || player.wingsLogic == 35 || player.wingsLogic == 37) && player.controlJump && player.controlDown && player.wingTime > 0f)
                flying = true;

            return flying;
        }

        public static bool AllMobilityDepleted(this Player player)
        {
            return player.AllJumpsDepleted() && player.wingTime <= 0 && player.rocketTime <= 0 && player.jump <= 0;
        }
    }
}
