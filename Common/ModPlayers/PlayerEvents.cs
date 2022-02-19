using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    public class PlayerEvents : ModPlayer
    {
        public delegate void OnHitNPCWithProjDelegate(Player player, Projectile projectile, NPC target, int damage, float knockback, bool crit);

        public static event OnHitNPCWithProjDelegate OnHitNPCWithProjEvent;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitNPCWithProjEvent?.Invoke(player, proj, target, damage, knockback, crit);
        }

        public static void Unload()
        {
            OnHitNPCWithProjEvent = null;
        }
    }
}
