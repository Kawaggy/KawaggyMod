using KawaggyMod.Core;
using KawaggyMod.Core.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content
{
    public class VanillaResprites
    {
        private const string TerrariaPath = "Terraria/Item_";
        public class MeleeResprites
        {
            private const string MeleePath = Assets.Resprites + "Vanilla/Melee/";
            public class Yoyo : INoServerLoadable
            {
                public void Load(Mod mod)
                {
                    Main.itemTexture[ItemID.WoodYoyo] = ModContent.GetTexture(MeleePath + "WoodenYoyo");
                    Main.itemTexture[ItemID.JungleYoyo] = ModContent.GetTexture(MeleePath + "AmazonYoyo");
                    Main.itemTexture[ItemID.Rally] = ModContent.GetTexture(MeleePath + "Rally");
                    Main.itemTexture[3279] = ModContent.GetTexture(MeleePath + "Malaise");
                }

                public void Unload(Mod mod)
                {
                    Main.itemTexture[ItemID.WoodYoyo] = ModContent.GetTexture(TerrariaPath + ItemID.WoodYoyo);
                    Main.itemTexture[ItemID.JungleYoyo] = ModContent.GetTexture(TerrariaPath + ItemID.JungleYoyo);
                    Main.itemTexture[ItemID.Rally] = ModContent.GetTexture(TerrariaPath + ItemID.Rally);
                    Main.itemTexture[3279] = ModContent.GetTexture(TerrariaPath + 3279);
                }
            }
        }

        public class MagicResprites
        {
            private const string MagicPath = Assets.Resprites + "Vanilla/Magic/";
            public class Books : INoServerLoadable
            {
                public void Load(Mod mod)
                {
                    Main.itemTexture[ItemID.WaterBolt] = ModContent.GetTexture(MagicPath + "WaterBolt");
                    Main.itemTexture[ItemID.BookofSkulls] = ModContent.GetTexture(MagicPath + "BookofSkulls");
                    Main.itemTexture[ItemID.DemonScythe] = ModContent.GetTexture(MagicPath + "DemonScythe");
                }

                public void Unload(Mod mod)
                {
                    Main.itemTexture[ItemID.WaterBolt] = ModContent.GetTexture(TerrariaPath + ItemID.WaterBolt);
                    Main.itemTexture[ItemID.BookofSkulls] = ModContent.GetTexture(TerrariaPath + ItemID.BookofSkulls);
                    Main.itemTexture[ItemID.DemonScythe] = ModContent.GetTexture(TerrariaPath + ItemID.DemonScythe);
                }
            }
        }
    }
}
