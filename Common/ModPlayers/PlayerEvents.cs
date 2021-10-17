using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    public class PlayerEvents : ModPlayer
    {
        public delegate void OnHitPlayer(Player player, Projectile projectile, NPC target, int damage, float knockback, bool crit);

        public static event OnHitPlayer OnHitPlayerEvent;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitPlayerEvent?.Invoke(player, proj, target, damage, knockback, crit);
        }

        public static void Unload()
        {
            OnHitPlayerEvent = null;
        }
    }
}
