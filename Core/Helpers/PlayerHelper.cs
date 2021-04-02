using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class PlayerHelper
    {
        public static void StealLife(this Player player, int damage, float percentage)
        {
            int life = (int)(damage * percentage);
            if (life < 1)
                life = 1;
            player.statLife += life;
            if (Main.myPlayer == player.whoAmI)
                player.HealEffect(life);
        }
    }
}
