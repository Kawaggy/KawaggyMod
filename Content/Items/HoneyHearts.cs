using KawaggyMod.Core;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Content.Items
{
    public abstract class HoneyHeartBase : KItem
    {
        public override string Texture => Assets.ExtraTextures + "YellowHeartItem";
        private readonly int healing;
        private readonly int time;

        public HoneyHeartBase(int healing, int time)
        {
            this.healing = healing;
            this.time = time;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{$Mods.KawaggyMod.Common.HoneyHeart}");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
        }

        public override bool OnPickup(Player player)
        {
            player.statLife += healing;
            player.AddBuff(BuffID.Honey, 60 * time);
            if (player.whoAmI == Main.myPlayer)
                player.HealEffect(healing);
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float pulse = Main.rand.Next(90, 111) * 0.01f;
            pulse *= Main.essScale * 0.5f;

            int X = (int)((item.position.X + (item.width / 2)) / 16f);
            int Y = (int)((item.position.Y + (item.height / 2)) / 16f);
            Lighting.AddLight(X, Y, 0.6f * pulse, 0.6f * pulse, 0f);
            
            if (item.spawnTime < int.MaxValue - 1)
                item.spawnTime += 5;
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            if (player.lifeMagnet)
                grabRange += 250;
        }
    }

    public class HoneyHeart_5HP : HoneyHeartBase
    {
        public HoneyHeart_5HP() : base(5, 5) { }
    }
}
