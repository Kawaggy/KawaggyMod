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
                "",
                "Additions:",
                " - Chilling Shard, a material and a bullet",
                " - Cracked Ice Orb, a summon weapon",
                " - Cloudy Candy, a summon weapon",
                " - Icy Book, a magic weapon",
                " - Burning Blood Dye",
                " - Death Whisper Dye",
                "",
                "Vanilla Changes: ",
                " - When a summon is active the players' damage on other weapons is reduced by 25% in pre-hardmode, 50% in hardmode and 75% in post-moonlord",
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
