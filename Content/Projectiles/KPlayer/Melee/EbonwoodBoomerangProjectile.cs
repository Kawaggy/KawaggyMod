using KawaggyMod.Content.Buffs.Debuffs;
using KawaggyMod.Core.ModTypes;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Melee
{
    public class EbonwoodBoomerangProjectile : ModBoomerang
    {
        public override string Texture => "KawaggyMod/Content/Items/Weapons/Melee/EbonwoodBoomerang";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebonwood Boomerang");
        }

        public override void SetDefaults()
        {
            Defaults(size: 26);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);

            target.AddBuff(ModContent.BuffType<CursedFlames>(), (int)(60 * 5f));
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 10;
            height = 10;
            fallThrough = true;
            return true;
        }
    }
}
