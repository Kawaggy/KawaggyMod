using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.Players
{
    public class KawaggyPlayerEvents : ModPlayer
    {
        public delegate void ModifyHitNPCWithProjDelegate(Player player, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection);
        public static event ModifyHitNPCWithProjDelegate ModifyHitNPCWithProjEvent;
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.type != NPCID.TargetDummy)
            {
                ModifyHitNPCWithProjEvent?.Invoke(player, proj, target, ref damage, ref knockback, ref crit, ref hitDirection);
            }
        }
    }
}
