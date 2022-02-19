using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Buffs.Summoner
{
    public class IceSwordsBuff : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

            DisplayName.SetDefault("Ice Sword Buff");
            Description.SetDefault("Magical Ice Swords are here to protect you!");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CheckProjectileExists(ModContent.ProjectileType<IceSwords>(), ref buffIndex);
        }
    }
}
