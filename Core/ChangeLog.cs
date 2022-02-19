using System.IO;
using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class ChangeLog
    {
        public static void GenerateOrUpdate(Mod mod)
        {
            string thePath = Path.Combine(KawaggyMod.SavePath, "ChangeLog") + ".txt";

            string changeLogVersion = KawaggyMod.Instance.Version.ToString();
            string[] lines =
            {
                $"Change log for v{changeLogVersion}",
                "Pre-hardmode expansion - First release!",
                "This update expands content, for the most part, in the Pre-hardmode area of the game.",
                "At the top will be a summary of additions. Below them will be a more detailed version.",
                "",
                "--------------------------------------------------------------------------------------------------------------------------------------------",
                "",
                " - Added 7 weapons",
                " - Added a material (which is also a bullet)",
                " - Added 3 other materials",
                " - Added an ore (which is currently unobtainable)",
                " - Added 2 debuffs",
                " - Added 3 Accessories (one with variants)",
                " - Added 2 dyes",
                " - Added a critter",
                " - Added an armor set",
                " - Changed vanilla behaviour while summons are active",
                "",
                "--------------------------------------------------------------------------------------------------------------------------------------------",
                "",
                "Additions:",
                "",
                " - Debuffs:",
                "  - Cursed Flames, a weaker Cursed Inferno",
                "  - Weaker Ichor, a weaker Ichor",
                "",
                " - Accessories:",
                "  - Sniper Bullet, a ranger accessory, of which it has 4 variants",
                "  - Mahogany Amulet, a summoner accessory",
                "  - Spirit Marked Bracelet, a summoner accessory",
                "",
                " - Dyes:",
                "  - Burning Blood",
                "  - Death Whisper",
                "",
                " - Materials",
                "  - Brain of Cthulhu Tooth",
                "  - Weater of Worlds Tooth",
                "  - Wyvern Feather",
                "  - Chilling Shards, which is also a bullet",
                "",
                " - Placeables:",
                "  - Lumenyx Ore",
                "",
                " - Weapons:",
                "  - Magic:",
                "   - Icy Book",
                "  - Melee:",
                "   - Ebonwood Boomerang",
                "   - Shadewood Boomerang",
                "  - Summoner:",
                "   - Cloudy Candy",
                "   - Cracked Ice Orb",
                "  - Throwing:",
                "   - Blighted Grenade",
                "   - Metastasize Ball",
                "",
                " - NPCs:",
                "  -Critters:",
                "   - Small Wyvern",
                "",
                " - Vanilla Changes: ",
                "  - When a summon is active the players' damage on other weapons is reduced by 25% in pre-hardmode, 50% in hardmode and 75% in post-moonlord"
            };

            if (!File.Exists(thePath))
            {
                mod.Logger.Info("ChangeLog did not exist! Generating...");
                File.WriteAllLines(thePath, lines);
            }
            else
            {
                mod.Logger.Info("ChangeLog did exist...");
                if (File.ReadAllLines(thePath)[0] != $"Change log for v{changeLogVersion}")
                {
                    File.WriteAllLines(thePath, lines);
                    mod.Logger.Info("...and is not up-to-date! Updating...");
                }
                else
                {
                    mod.Logger.Info("...and is up-to-date!");
                }
            }
        }
    }
}
