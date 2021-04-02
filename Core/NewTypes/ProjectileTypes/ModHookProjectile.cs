using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.NewTypes.ProjectileTypes
{
    public abstract class ModHookProjectile : KProjectile
    {
        private readonly int HooksMax;
        private readonly int HooksOutMax;
        private readonly float HookLength;
        private readonly float RetreatSpeed;
        private readonly float PullSpeed;

        public ModHookProjectile(int maxHooks, int maxHooksOut, float hookLength, float retreatSpeed, float pullSpeed)
        {
            HooksMax = maxHooks;
            HooksOutMax = maxHooksOut;
            HookLength = hookLength;
            RetreatSpeed = retreatSpeed;
            PullSpeed = pullSpeed;
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            SafeSetDefaults();
        }

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            int hooksOnTile = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projectile.type)
                {
                    if (Main.projectile[i].ai[0] != 2)
                    {
                        if (OnlyOneHookOut)
                            return false;
                    }
                    else
                        hooksOnTile++;
                    hooksOut++;
                }
            }
            if (hooksOnTile == HooksOutMax && hooksOut <= HooksOutMax)
                return true;
            if (hooksOut > HooksOutMax - 1)
                return false;
            return true;
        }

        public virtual bool OnlyOneHookOut => false;
        public virtual string TexturePath => null;

        public override float GrappleRange() => HookLength;

        public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = HooksMax;

        public override void GrapplePullSpeed(Player player, ref float speed) => speed = PullSpeed;

        public override void GrappleRetreatSpeed(Player player, ref float speed) => speed = RetreatSpeed;

        public override bool PreDrawExtras(SpriteBatch spriteBatch) => projectile.DrawHook(Main.player[projectile.owner].MountedCenter, spriteBatch, TexturePath ?? (Texture == Assets.NoTexture ? Texture : Texture + "_Chain"));
    }

}
